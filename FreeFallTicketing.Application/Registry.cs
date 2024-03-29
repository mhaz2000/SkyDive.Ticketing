﻿using SkyDiveTicketing.Application.Services.FileServices;
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
using SkyDiveTicketing.Application.Services.UserMessageServices;
using SkyDiveTicketing.Application.Services.AdminCartableServices;
using SkyDiveTicketing.Application.Services.ReportServices;
using SkyDiveTicketing.Application.Services.ShoppingCartCheckoutServices;
using SkyDiveTicketing.Application.PaymentServices;

namespace SkyDiveTicketing.Application
{
    public static class Registry
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IUserTypeService, UserTypeService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IPassengerService, PassengerService>();
            services.AddScoped<IJumpRecordService, JumpRecordService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IUserMessageService, UserMessageService>();
            services.AddScoped<ISkyDiveEventService, SkyDiveEventService>();
            services.AddScoped<IAdminCartableService, AdminCartableService>();
            services.AddScoped<IZarinpalPaymentService, ZarinpalPaymentService>();
            services.AddScoped<ISkyDiveEventStatusService, SkyDiveEventStatusService>();
            services.AddScoped<IShoppingCartCheckoutService, ShoppingCartCheckoutService>();
            services.AddScoped<ISkyDiveEventTicketTypeService, SkyDiveEventTicketTypeService>();
            services.AddScoped<IFlightLoadCancellationTypeService, FlightLoadCancellationTypeService>();
        }
    }
}
