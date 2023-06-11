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
            Expression<Func<ShoppingCart, object>>[] includeExpressions = {
                c=> c.User,
                c=> c.Tickets,
            };

            var shoppingCart = await _unitOfWork.ShoppingCartRepository.GetFirstWithIncludeAsync(c => c.User.Id == userId, includeExpressions);
            if (shoppingCart is null || !shoppingCart.Tickets.Any())
                throw new ManagedException("سبد خرید شما خالی است.");

            var unLockedTickets = shoppingCart.Tickets.Where(c => !c.Locked);
            if (unLockedTickets.Any())
            {
                List<string> errors = new List<string>();

                Dictionary<FlightLoadItem, List<Ticket>> ticketItems = new Dictionary<FlightLoadItem, List<Ticket>>();
                foreach (var ticket in unLockedTickets)
                {
                    var flightLoadItem = await _unitOfWork.FlightLoadRepository.GetFlightItemByTicket(ticket);
                    if (ticketItems.ContainsKey(flightLoadItem))
                        ticketItems[flightLoadItem].Add(ticket);
                }

                foreach (var item in ticketItems)
                {
                    var flightLoad = await _unitOfWork.FlightLoadRepository.GetFirstWithIncludeAsync(c => c.FlightLoadItems.Contains(item.Key), c => c.FlightLoadItems);
                    if (item.Key.SeatNumber - (item.Key.Tickets.Where(c => !c.Locked && !c.Cancelled).Count() + item.Value.Count()) < 0)
                        errors.Add($"برای پرواز شماره {flightLoad.Number} بلیت {item.Key.FlightLoadType.Title} به میزان درخواستی وجود ندارد.");
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
                c=> c.Tickets,
            };

            var shoppingCart = await _unitOfWork.ShoppingCartRepository.GetFirstWithIncludeAsync(c => c.User.Id == userId, includeExpressions);
            if (shoppingCart is null)
                throw new ManagedException("سبد خرید شما خالی است.");

            //check if paid

            var ticketModels = _unitOfWork.SkyDiveEventRepository.GetDetails();

            foreach (var ticket in shoppingCart.Tickets)
            {
                var ticketModel = ticketModels.FirstOrDefault(x => x.Ticket == ticket);
                _unitOfWork.TicketRepository.SetAsPaid(ticket, ticketModel.SkyDiveEvent.TypesAmount.FirstOrDefault(c => c.Type.Id == ticketModel.FlightLoadItem.FlightLoadType.Id).Amount);

                await _unitOfWork.TransactionRepositroy.AddTransaction(ticket.TicketNumber,
                    ticketModel.SkyDiveEvent.Location + "کد" + ticketModel.SkyDiveEvent.Code.ToString("000"), "", 0, TransactionType.Confirmed, user);
            }

            await _unitOfWork.ShoppingCartRepository.ClearShoppingCartAsync(user);

            return true;

        }

        public async Task UnlockTickets()
        {
            var tickets = _unitOfWork.TicketRepository.AsEnumerable().Where(x => Math.Abs((DateTime.Now - x.CreatedAt).TotalMinutes) >= 15);

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
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            List<string> errors = new List<string>();
            List<Ticket> tickets = new List<Ticket>();

            _unitOfWork.TicketRepository.ClearUserTicket(user);

            foreach (var item in command.Items)
            {
                var skyDiveEvent = _unitOfWork.SkyDiveEventRepository.FindEvents(c => c.Items.Any(c => c.Id == item.SkyDiveItemId)).FirstOrDefault();
                if (skyDiveEvent is null)
                    throw new ManagedException("رویدادی پیدا نشد.");

                var skyDiveEventItem = skyDiveEvent.Items.FirstOrDefault(c => c.Id == item.SkyDiveItemId);
                var flightLoad = skyDiveEventItem.FlightLoads.FirstOrDefault(c => c.FlightLoadItems.Any(t => t.Id == item.FlightLoadItemId));
                var flightLoadItem = flightLoad.FlightLoadItems.FirstOrDefault(c => c.Id == item.FlightLoadItemId);

                if (flightLoadItem.SeatNumber - (flightLoadItem.Tickets.Where(c => !c.Locked && !c.Cancelled).Count() + item.Qty) < 0)
                    errors.Add($"برای پرواز شماره {flightLoad.Number} بلیت {flightLoadItem.FlightLoadType.Title} به میزان درخواستی وجود ندارد.");
                else
                {
                    for (int i = 0; i < item.Qty; i++)
                        tickets.Add(_unitOfWork.TicketRepository.AddTicket(flightLoadItem, user, flightLoad.Number, skyDiveEvent));
                }
            }

            if (errors.Any())
                throw new ManagedException(string.Join("\n", errors));

            await _unitOfWork.ShoppingCartRepository.ClearShoppingCartAsync(user);
            await _unitOfWork.ShoppingCartRepository.AddToShoppingCart(user, tickets);
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
