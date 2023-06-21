using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.Reservation;
using SkyDiveTicketing.Application.DTOs.TicketDTOs;
using SkyDiveTicketing.Application.Helpers;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;
using System.Globalization;
using System.Linq.Expressions;

namespace SkyDiveTicketing.Application.Services.ReservationServices
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReservationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(bool, string)> CheckTickets(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربر یافت نشد.");

            var shoppingCart = await _unitOfWork.ShoppingCartRepository.GetUserShoppingCart(user);
            if (shoppingCart is null || !shoppingCart.Items.Any())
                throw new ManagedException("سبد خرید شما خالی است.");

            var shoppingCartItems = shoppingCart.Items;
            if (shoppingCartItems.Any())
            {
                List<string> errors = new List<string>();

                foreach (var shoppingCartItem in shoppingCartItems)
                {
                    var flightLoad = await _unitOfWork.FlightLoadRepository.GetFlightLoadByItem(shoppingCartItem.FlightLoadItem);
                    if (flightLoad is null)
                        throw new ManagedException("پرواز مورد نظر یافت نشد.");

                    var availableTickets = shoppingCartItem.FlightLoadItem.Tickets.Where(c => (!c.Locked || c.LockedBy.Id == user.Id) && !c.ReservedByAdmin && c.ReservedBy is null);
                    if(availableTickets.Count() < shoppingCartItem.Qty)
                        errors.Add($"برای پرواز شماره {flightLoad.Number} بلیت {shoppingCartItem.FlightLoadItem.FlightLoadType.Title} به میزان درخواستی وجود ندارد.");
                }

                if (errors.Any())
                    return (false, string.Join("\n", errors));
            }

            return (true, string.Empty);
        }

        public async Task<IEnumerable<MyTicketDTO>> GetUserTickets(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            List<MyTicketDTO> myTickets = new List<MyTicketDTO>();
            var ticketModels = _unitOfWork.SkyDiveEventRepository.GetDetails();

            var tickets = _unitOfWork.TicketRepository.Include(c => c.ReservedBy).Where(c => c.ReservedBy == user);
            foreach (var ticket in tickets)
            {
                var ticketModel = ticketModels.FirstOrDefault(x => x.Ticket == ticket);

                myTickets.Add(new MyTicketDTO(ticket.Id, ticket.CreatedAt, ticket.UpdatedAt, ticket.TicketNumber, ticketModel.FlightLoad.Date, ticketModel.FlightLoad.Number.ToString("000"),
                    ticketModel.SkyDiveEvent.Location, ticketModel.FlightLoadItem.FlightLoadType.Title, ticketModel.SkyDiveEvent.TermsAndConditions, ticketModel.SkyDiveEvent.Voidable));
            }

            return myTickets;
        }

        public async Task<bool> SetAsPaid(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            Expression<Func<ShoppingCart, object>>[] includeExpressions = {
                c=> c.User,
                c=> c.Items,
            };

            var shoppingCart = await _unitOfWork.ShoppingCartRepository.GetFirstWithIncludeAsync(c => c.User.Id == userId, includeExpressions);
            if (shoppingCart is null)
                throw new ManagedException("سبد خرید شما خالی است.");

            //check if paid

            var ticketModels = _unitOfWork.SkyDiveEventRepository.GetDetails();

            foreach (var shoppingCartTicket in shoppingCart.Items)
            {
                //var ticketModel = ticketModels.FirstOrDefault(x => x.Ticket == shoppingCartTicket.Ticket);
                //_unitOfWork.TicketRepository.SetAsPaid(shoppingCartTicket.Ticket,
                //    ticketModel.SkyDiveEvent.TypesAmount.FirstOrDefault(c => c.Type.Id == ticketModel.FlightLoadItem.FlightLoadType.Id).Amount);

                //await _unitOfWork.TransactionRepositroy.AddTransaction(shoppingCartTicket.Ticket.TicketNumber,
                //    ticketModel.SkyDiveEvent.Location + "کد" + ticketModel.SkyDiveEvent.Code.ToString("000"), "", 0, TransactionType.Confirmed, user);
            }

            await _unitOfWork.ShoppingCartRepository.ClearShoppingCartAsync(user);

            return true;

        }

        public async Task UnlockTickets()
        {
            var tickets = _unitOfWork.TicketRepository.Include(c=>c.LockedBy).AsEnumerable()
                .Where(x => x.ReserveTime is not null && Math.Abs((DateTime.Now - x.ReserveTime.Value).TotalMinutes) >= 15);

            foreach (var ticket in tickets)
                _unitOfWork.TicketRepository.Unlock(ticket);

            await _unitOfWork.CommitAsync();
        }

        public async Task CancelTicketRequest(Guid id, Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            var ticket = await _unitOfWork.TicketRepository.GetFirstWithIncludeAsync(c => c.Id == id, c => c.RelatedAdminCartableRequest);
            if (ticket is null)
                throw new ManagedException("بلیت مورد نظر یافت نشد.");

            if (!ticket.Voidable)
                throw new ManagedException("امکان کنسل کردن بلیت وجود ندارد.");

            await _unitOfWork.AdminCartableRepository.AddToCartable("در خواست کنسلی بلیت", user, RequestType.TicketCancellation, ticket);
            await _unitOfWork.CommitAsync();
        }

        public async Task Update(ReserveCommand command, Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetUserWithInclude(c => c.Id == userId);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            List<string> errors = new List<string>();
            Dictionary<FlightLoadItem, int> flightLoadItems = new Dictionary<FlightLoadItem, int>();

            _unitOfWork.TicketRepository.ClearUserTicket(user);

            //var skyDiveEvents = _unitOfWork.SkyDiveEventRepository.FindEvents(c => c.IsActive).AsEnumerable();
            var skyDiveEvents = _unitOfWork.SkyDiveEventRepository.FindEvents().AsEnumerable();

            foreach (var item in command.Items)
            {
                var ticketType = await _unitOfWork.SkyDiveEventTicketTypeRepository.GetByIdAsync(item.TicketTypeId);
                if (ticketType is null)
                    throw new ManagedException("نوع بلیت مورد نظر وجود ندارد.");

                if (!user.UserType.AllowedTicketTypes.Any(c => c.TicketTypeId == item.TicketTypeId))
                    errors.Add($"نوع کاربری شما مجاز به رزرو بلیت های نوع '{ticketType.Title}' نیست.");

                SkyDiveEventItem skyDiveEventItem = null;

                var skyDiveEvent = skyDiveEvents
                    .Where(skyDiveEvent =>
                    {
                        skyDiveEventItem = skyDiveEvent.Items.FirstOrDefault(day => day.FlightLoads.Any(flight => flight.Id == item.FlightLoadId));
                        if (skyDiveEventItem is null)
                            throw new ManagedException("پرواز مورد نظر یافت نشد.");

                        return true;

                    }).FirstOrDefault();

                if (skyDiveEvent is null)
                    throw new ManagedException("رویدادی پیدا نشد.");

                var flightLoad = skyDiveEventItem.FlightLoads.FirstOrDefault(c => c.Id == item.FlightLoadId);
                var flightLoadItem = flightLoad.FlightLoadItems.FirstOrDefault(c => c.FlightLoadType.Id == item.TicketTypeId);

                var availableTickets = flightLoadItem.Tickets.Where(c => !c.Locked && !c.Cancelled && !c.ReservedByAdmin).Take(item.Qty).ToList();
                if (availableTickets is null || !availableTickets.Any())
                    errors.Add($"برای پرواز شماره {flightLoad.Number} بلیت {flightLoadItem.FlightLoadType.Title} به میزان درخواستی وجود ندارد.");
                else
                {
                    foreach (var ticket in availableTickets)
                        _unitOfWork.TicketRepository.ReserveTicket(ticket, user);

                    flightLoadItems.Add(flightLoadItem, item.Qty);
                }
            }

            if (errors.Any())
                throw new ManagedException(string.Join("\n", errors));

            await _unitOfWork.ShoppingCartRepository.ClearShoppingCartAsync(user);
            await _unitOfWork.ShoppingCartRepository.AddToShoppingCart(user, flightLoadItems);
            await _unitOfWork.CommitAsync();

        }

        public async Task CancelTicketResponse(Guid id, bool response)
        {
            var request = await _unitOfWork.AdminCartableRepository.GetByIdAsync(id);
            if (request is null)
                throw new ManagedException("درخواست مورد نظر یافت نشد.");

            var ticket = await _unitOfWork.TicketRepository.GetFirstWithIncludeAsync(c => c.RelatedAdminCartableRequest == request, c => c.RelatedAdminCartableRequest);

            if (response)
            {
                _unitOfWork.TicketRepository.SetAsCancelled(ticket);
                _unitOfWork.UserRepository.AddMessage(request.Applicant, $"در خواست لغو بلیت با شماره {ticket.TicketNumber} توسط ادمین تایید شد.");

                var wallet = await _unitOfWork.WalletRepository.GetFirstWithIncludeAsync(c => c.User == request.Applicant, c => c.User);
                if (wallet is not null)
                    _unitOfWork.WalletRepository.ChangeWalletBalance(wallet, ticket.PaidAmount);
            }
            else
                _unitOfWork.UserRepository.AddMessage(request.Applicant, $"در خواست لغو بلیت با شماره {ticket.TicketNumber} توسط ادمین رد شد.");

            await _unitOfWork.CommitAsync();
        }

        public MemoryStream PrintTicket(Guid id)
        {
            PersianCalendar pc = new PersianCalendar();

            var ticket = _unitOfWork.SkyDiveEventRepository.GetDetails(c => c.Ticket.Id == id).FirstOrDefault();
            if (ticket is null)
                throw new ManagedException("بلیت مورد نظر یافت نشد.");

            return PdfHelper.TicketPdf(ticket.Ticket.ReservedBy.FullName, ticket.FlightLoadItem.FlightLoadType.Title, ticket.Ticket.TicketNumber,
                ticket.SkyDiveEvent.Location, $"{pc.GetYear(ticket.FlightLoad.Date)}/{pc.GetMonth(ticket.FlightLoad.Date)}/{pc.GetDayOfMonth(ticket.FlightLoad.Date)}",
                ticket.FlightLoad.Number.ToString("000"), ticket.Ticket.ReservedBy.NationalCode ?? string.Empty);
        }
    }
}
