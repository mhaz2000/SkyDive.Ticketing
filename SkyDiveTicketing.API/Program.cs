using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.API.Extensions;
using SkyDiveTicketing.Application;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Hangfire;
using System.Text;
using SkyDiveTicketing.Application.Helpers;
using SkyDiveTicketing.API.Jobs.PassengerDocumentJobs;
using SkyDiveTicketing.API.Jobs.TicketJobs;
using System.Text.Json.Serialization;
using SkyDiveTicketing.API.Jobs.JumpRecordJobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Main")));
builder.Services.AddDbContext<LogContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LogDb")));

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new PersianDateTimeConverter());
    opt.JsonSerializerOptions.Converters.Add(new DictionaryTKeyEnumTValueConverter());
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .Build();

AppSettingsModel appSettingsModel = config.Get<AppSettingsModel>();
var jwtIssuerOptions = appSettingsModel.JwtIssuerOptions;

SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtIssuerOptions.SecretKey));

// Configure JwtIssuerOptions
builder.Services.Configure<JwtIssuerOptionsModel>(options =>
{
    options.Issuer = jwtIssuerOptions.Issuer;
    options.Audience = jwtIssuerOptions.Audience;
    options.SecretKey = jwtIssuerOptions.SecretKey;
    options.ExpireTimeTokenInMinute = jwtIssuerOptions.ExpireTimeTokenInMinute;
    options.ValidTimeInMinute = jwtIssuerOptions.ValidTimeInMinute;
    options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
});

// add identity
var identityBuilder = builder.Services.AddIdentity<User, IdentityRole<Guid>>(o =>
{
    // configure identity options
    o.Password.RequireDigit = true;
    o.Password.RequireLowercase = true;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 6;
    o.Tokens.ChangePhoneNumberTokenProvider = "Phone";
});

identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(IdentityRole<Guid>), builder.Services);

identityBuilder.AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
identityBuilder.AddEntityFrameworkStores<LogContext>().AddDefaultTokenProviders();


var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = jwtIssuerOptions.Issuer,

    ValidateAudience = true,
    ValidAudience = jwtIssuerOptions.Audience,

    ValidateIssuerSigningKey = true,
    IssuerSigningKey = signingKey,

    RequireExpirationTime = false,
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions =>
{
    configureOptions.ClaimsIssuer = jwtIssuerOptions.Issuer;
    configureOptions.TokenValidationParameters = tokenValidationParameters;
    configureOptions.SaveToken = true;
});


builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<IJwtFactory, JwtFactory>();
builder.Services.AddScoped<ITokenFactory, TokenFactory>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPassengerDocumentJob, PassengerDocumentJob>();
builder.Services.AddScoped<ITicketJob, TicketJob>();
builder.Services.AddScoped<ILogUnitOfWork, LogUnitOfWork>();
builder.Services.AddScoped<ExceptionLogger>();



builder.Services.AddHangfire(configuration => configuration
             .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
             .UseSimpleAssemblyNameTypeSerializer()
             .UseRecommendedSerializerSettings()
             .UseSqlServerStorage(config.GetConnectionString("Main")));

builder.Services.AddHangfireServer();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin",
        authBuilder =>
        {
            authBuilder.RequireRole("Admin");
        });

    options.AddPolicy("User",
        authBuilder =>
        {
            authBuilder.RequireRole("User");
        });
});


Registry.Register(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    //app.UseSwagger();
//    //app.UseSwaggerUI();
//}
    app.UseHangfireDashboard();

app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});


app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MigrateDatabase<Program>();

RecurringJob.AddOrUpdate<IPassengerDocumentJob>("ExpiredDocument", c => c.CheckPassengerDocumentExpirationDate(), Cron.Daily);
RecurringJob.AddOrUpdate<ITicketJob>("UnlockTicket", c => c.CheckTicketLockTime(), Cron.Minutely);
RecurringJob.AddOrUpdate<IJumpRecordJob>("ExpiredJumpRecord", c => c.CheckIfExpired(), Cron.Daily);

app.Run();
