﻿using SkyDiveTicketing.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkyDiveTicketing.Infrastructure.Data;
using System.Text.Json;

namespace SkyDiveTicketing.API.Extensions
{
    public static class HostExtensions
    {
        private static readonly MyDbContextFactory MyDbContextFactory = new MyDbContextFactory();
        private static IUserStore<User> _userStore;
        private static UserManager<User> _userManager;
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();
                try
                {
                    var dataContext = MyDbContextFactory.CreateDbContext(new[] { string.Empty });

                    _userStore = new UserStore<User, IdentityRole<Guid>, DataContext, Guid>(dataContext);
                    _userManager = new UserManager<User>(_userStore, null, new PasswordHasher<User>(), null, null, null, null, null, null);
                    dataContext.Database.Migrate();
                    CreateAdminSeed(dataContext, _userManager);
                    CreateRolesSeed(dataContext);
                    SeedCities(dataContext);
                    CreateUserType(dataContext);
                    CreateSkyDiveEventTicketType(dataContext);

                    logger.LogInformation("Migrating database");
                }
                catch (Exception ex)
                {
                    logger.LogError("An error has been occured\n" + ex);

                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryForAvailability);
                    }
                }
            }

            return host;
        }

        private static void SeedCities(DataContext dataContext)
        {
            if (!dataContext.Cities.Any())
            {
                var filePath = Directory.GetCurrentDirectory() + @"JsonFiles\cities.json";
                using FileStream stream = File.OpenRead(filePath);
                var cities = JsonSerializer.Deserialize<ICollection<DefaultCity>>(stream);

                dataContext.Cities.AddRange(cities);
                dataContext.SaveChanges();
            }
        }

        private static void CreateAdminSeed(DataContext context, UserManager<User> userManager)
        {
            string username = "admin";

            if (!context.Users.Any(u => u.UserName == username))
            {
                var newUser = new User()
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    UserName = "admin",
                    NormalizedUserName = "admin"
                };

                var done = userManager.CreateAsync(newUser, "123456");

                if (done.Result.Succeeded)
                    userManager.AddToRoleAsync(newUser, "Admin");

                context.SaveChanges();
            }
        }

        private static void CreateRolesSeed(DataContext context)
        {
            var role = context.Roles.AnyAsync(c => c.Name == "Admin");

            if (!role.Result)
            {
                var newAdminRole = new IdentityRole()
                {
                    Name = "Admin",
                    Id = Guid.NewGuid().ToString(),
                    NormalizedName = "admin"
                };

                context.Roles.Add(newAdminRole);

                var newUserRole = new IdentityRole()
                {
                    Name = "User",
                    Id = Guid.NewGuid().ToString(),
                };
                context.Roles.Add(newUserRole);

                context.SaveChanges();
            }
        }

        private static void CreateUserType(DataContext context)
        {
            if (!context.UserTypes.Any())
            {
                context.UserTypes.Add(new UserType("تعیین نشده") { IsDefault = true });
                context.SaveChanges();
            }
        }

        private static void CreateSkyDiveEventTicketType(DataContext context)
        {
            if (!context.SkyDiveEventTicketTypes.Any())
            {
                context.SkyDiveEventTicketTypes.Add(new SkyDiveEventTicketType("آزاد", 1));
                context.SkyDiveEventTicketTypes.Add(new SkyDiveEventTicketType("ویژه", 3));
                context.SkyDiveEventTicketTypes.Add(new SkyDiveEventTicketType("با مربی", 2));

                context.SaveChanges();
            }
        }
    }
}
