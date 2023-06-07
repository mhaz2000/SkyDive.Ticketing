using SkyDiveTicketing.Application.Services.FileServices;
using SkyDiveTicketing.Application.Services.FlightLoadServices;
using SkyDiveTicketing.Application.Services.ReservationServices;
using SkyDiveTicketing.Application.Services.UserServices;
using Microsoft.Extensions.DependencyInjection;
using SkyDiveTicketing.Application.Services.UserTypeServices;
using SkyDiveTicketing.Application.Services.PassengerServices;
using SkyDiveTicketing.Application.Services.SkyDiveEventServices;
using SkyDiveTicketing.Application.Services.SettingsServices;
using SkyDiveTicketing.Application.Services.TransactionServices;

namespace SkyDiveTicketing.Application
{
    public static class Registry
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISkyDiveEventTicketTypeService, SkyDiveEventTicketTypeService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IFlightLoadCancellationTypeService, FlightLoadCancellationTypeService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IUserTypeService ,UserTypeService>();
            services.AddScoped<IPassengerService ,PassengerService>();
            services.AddScoped<ISkyDiveEventStatusService, SkyDiveEventStatusService>();
            services.AddScoped<ISkyDiveEventService, SkyDiveEventService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<ITransactionService, TransactionService>();
        }
    }
}
