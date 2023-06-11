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
using SkyDiveTicketing.Application.Services.JumpRecordServices;
using SkyDiveTicketing.Application.Services.WalletServices;

namespace SkyDiveTicketing.Application
{
    public static class Registry
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IUserTypeService, UserTypeService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IPassengerService, PassengerService>();
            services.AddScoped<IJumpRecordService, JumpRecordService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ISkyDiveEventService, SkyDiveEventService>();
            services.AddScoped<ISkyDiveEventStatusService, SkyDiveEventStatusService>();
            services.AddScoped<ISkyDiveEventTicketTypeService, SkyDiveEventTicketTypeService>();
            services.AddScoped<IFlightLoadCancellationTypeService, FlightLoadCancellationTypeService>();
        }
    }
}
