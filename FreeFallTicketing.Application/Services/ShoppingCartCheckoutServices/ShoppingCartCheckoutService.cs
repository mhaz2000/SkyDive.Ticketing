using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.DTOs.ShoppingCartDTOs;
using SkyDiveTicketing.Application.PaymentServices;
using SkyDiveTicketing.Application.Services.ReservationServices;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Application.Services.ShoppingCartCheckoutServices
{
    public class ShoppingCartCheckoutService : IShoppingCartCheckoutService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        private readonly IReservationService _reservationService;

        public ShoppingCartCheckoutService(IUnitOfWork unitOfWork, IZarinpalPaymentService zarinpalPaymentService, IReservationService reservationService)
        {
            _unitOfWork = unitOfWork;
            _paymentService = zarinpalPaymentService;
            _reservationService = reservationService;
        }

        public async Task<string> Checkout(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربر یافت نشد.");

            var shoppingCart = await _unitOfWork.ShoppingCartRepository.GetUserShoppingCart(user);

            if (!shoppingCart!.Items.Any())
                throw new ManagedException("سبد خرید خالی است.");

            var amount = await CalculateAmount(shoppingCart, user);

            var url = await _paymentService.Checkout(amount);

            return url;
        }

        private async Task<double> CalculateAmount(ShoppingCart shoppingCart, User user)
        {
            var settings = await _unitOfWork.SettingsRepository.FirstOrDefaultAsync(c => true);

            double payableAmount = 0;
            foreach (var item in shoppingCart.Items)
            {
                var ticket = item.FlightLoadItem.Tickets.FirstOrDefault(c => !c.Paid && !c.ReservedByAdmin && (!c.Locked || c.LockedBy == user));
                if (ticket is null)
                    throw new ManagedException("بلیت مورد نظر یافت نشد.");

                var ticketAmount = shoppingCart.SkyDiveEvent!.TypesAmount.FirstOrDefault(c => c.Type == item.FlightLoadItem.FlightLoadType)!.Amount;
                payableAmount += ticketAmount + (shoppingCart.SkyDiveEvent.SubjecToVAT ? Math.Round((ticketAmount * settings.VAT) / 100) : 0);
            }

            return payableAmount;
        }

        public async Task<PaidShoppingCartDTO> Verfiy(Guid userId, string authority)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربر یافت نشد.");

            var shoppingCart = await _unitOfWork.ShoppingCartRepository.GetUserShoppingCart(user);

            if (!shoppingCart!.Items.Any())
                throw new ManagedException("سبد خرید خالی است.");

            var shoppingCartInfo = await _reservationService.GetUserShoppingCart(userId);

            var amount = await CalculateAmount(shoppingCart, user);

            var refId = await _paymentService.Verify(authority, amount);

            await _reservationService.SetAsPaid(userId);

            await AddTransactions(shoppingCartInfo, user, amount, refId);

            await _unitOfWork.CommitAsync();

            return new PaidShoppingCartDTO(refId.ToString())
            {
                Items = shoppingCartInfo.Items,
                SkyDiveEventId = shoppingCartInfo.SkyDiveEventId,
                TaxAmount = shoppingCartInfo.TaxAmount,
                TotalAmount = shoppingCartInfo.TotalAmount
            };
        }

        private async Task AddTransactions(ShoppingCartDTO shoppingCart, User user, double amount, ulong refId)
        {
            var skyDiveEvent = await _unitOfWork.SkyDiveEventRepository.FirstOrDefaultAsync(c=> c.Id == shoppingCart.SkyDiveEventId);

            shoppingCart.Items.SelectMany(item => item.TicketsNumber, (item, ticketNumber) =>
                _unitOfWork.TransactionRepositroy.AddTransaction(ticketNumber, skyDiveEvent!.Title, refId.ToString(), amount, TransactionType.Confirmed, user, false));
        }
    }
}