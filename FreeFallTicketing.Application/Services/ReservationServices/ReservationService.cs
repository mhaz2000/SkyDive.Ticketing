using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.Reservation;
using SkyDiveTicketing.Application.DTOs.ShoppingCartDTOs;
using SkyDiveTicketing.Application.DTOs.TicketDTOs;
using SkyDiveTicketing.Application.Helpers;
using SkyDiveTicketing.Application.TempModels;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;
using System.Globalization;
using System.Linq;
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

                foreach (var shoppingCartItem in shoppingCartItems.GroupBy(c => c.FlightLoadItem))
                {
                    var flightLoad = await _unitOfWork.FlightLoadRepository.GetFlightLoadByItem(shoppingCartItem.Key);
                    if (flightLoad is null)
                        throw new ManagedException("پرواز مورد نظر یافت نشد.");

                    var availableTickets = shoppingCartItem.Key.Tickets.Where(c => (!c.Locked || c.LockedBy.Id == user.Id) && !c.ReservedByAdmin && c.ReservedBy is null);
                    if (availableTickets.Count() < shoppingCartItem.Count())
                        errors.Add($"برای پرواز شماره {flightLoad.Number} بلیت {shoppingCartItem.Key.FlightLoadType.Title} به میزان درخواستی وجود ندارد.");
                }

                if (errors.Any())
                    return (false, string.Join("\n", errors));
            }

            return (true, string.Empty);
        }

        public async Task<IEnumerable<MyTicketDTO>> GetUserTickets(Guid userId, string? statuses = null)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            List<MyTicketDTO> myTickets = new List<MyTicketDTO>();

            var tickets = _unitOfWork.TicketRepository.Include(c => c.ReservedBy!).Where(c => c.ReservedBy == user && c.Paid);
            foreach (var ticket in tickets)
            {
                var skyDiveEvent = await _unitOfWork.SkyDiveEventRepository.GetByIdAsync(ticket.SkyDiveEventId!.Value);

                myTickets.Add(new MyTicketDTO(ticket.Id, ticket.CreatedAt, ticket.UpdatedAt, ticket.TicketNumber, ticket.FlightDate!.Value,
                    ticket.FlightNumber!.Value.ToString("000"), skyDiveEvent.Location, ticket.TicketType!, skyDiveEvent.TermsAndConditions!,
                    skyDiveEvent.Voidable, skyDiveEvent.Id, skyDiveEvent.Code, skyDiveEvent.StartDate >= DateTime.Now ? TicketStatus.Held : TicketStatus.Reserved));
            }

            var cancelledTickets = _unitOfWork.TicketRepository.GetCancelledTickets().Where(c => c.ReservedBy == user);
            foreach (var ticket in cancelledTickets)
            {
                var skyDiveEvent = await _unitOfWork.SkyDiveEventRepository.GetByIdAsync(ticket.SkyDiveEventId!.Value);

                myTickets.Add(new MyTicketDTO(ticket.Id, ticket.CreatedAt, ticket.UpdatedAt, ticket.TicketNumber, ticket.FlightDate.Value,
                    ticket.FlightNumber.Value.ToString("000"), skyDiveEvent.Location, ticket.TicketType, skyDiveEvent.TermsAndConditions,
                    skyDiveEvent.Voidable, skyDiveEvent.Id, skyDiveEvent.Code, TicketStatus.Cancelled));
            }

            if (statuses is not null)
            {
                List<TicketStatus> ticketStatuses = new List<TicketStatus>();
                statuses.Split("|").ToList().ForEach(status =>
                {
                    if (Enum.TryParse(status, out TicketStatus ticketStatus))
                        ticketStatuses.Add(ticketStatus);
                    else
                        throw new ManagedException("فیلتر معتبر نیست.");
                });

                myTickets = myTickets.Where(c => ticketStatuses.Contains(c.TicketStatus)).ToList();
            }

            return myTickets;
        }

        public async Task<bool> SetAsPaid(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            var shoppingCart = await _unitOfWork.ShoppingCartRepository.GetUserShoppingCart(user);
            if (shoppingCart is null)
                throw new ManagedException("سبد خرید شما خالی است.");

            //check if paid

            await ReservationProcess(shoppingCart, user);

            return true;
        }

        public async Task<bool> SetAsPaidByWallet(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            var shoppingCart = await _unitOfWork.ShoppingCartRepository.GetUserShoppingCart(user);
            if (shoppingCart is null)
                throw new ManagedException("سبد خرید شما خالی است.");

            var wallet = await _unitOfWork.WalletRepository.GetFirstWithIncludeAsync(c => c.User.Id == userId, c => c.User);

            double payableAmount = await ReservationProcess(shoppingCart, user);
            if (wallet.Balance < payableAmount)
                throw new ManagedException("موجودی کیف پول شما کافی نیست.");
            
            _unitOfWork.WalletRepository.ChangeWalletBalance(wallet, payableAmount * (-1));

            return true;
        }


        public async Task UnlockTickets()
        {
            var tickets = _unitOfWork.TicketRepository.Include(c => c.LockedBy!).AsEnumerable()
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

            var ticket = await _unitOfWork.TicketRepository.GetFirstWithIncludeAsync(c => c.Id == id, c => c.RelatedAdminCartableRequest, c => c.PaidBy);
            if (ticket is null)
                throw new ManagedException("بلیت مورد نظر یافت نشد.");

            if (!ticket.Voidable || ticket.ReservedBy != user)
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
            List<Tuple<FlightLoadItem, User>> flightLoadItems = new List<Tuple<FlightLoadItem, User>>();

            UnlockShoppingCartItems(user);

            if (!command.Items.Any())
                await _unitOfWork.ShoppingCartRepository.RemoveUserShoppingCart(user);
            else
            {
                //var skyDiveEvents = _unitOfWork.SkyDiveEventRepository.FindEvents(c => c.IsActive).AsEnumerable();
                var skyDiveEvents = _unitOfWork.SkyDiveEventRepository.FindEvents().AsEnumerable();

                SkyDiveEvent? skyDiveEvent = null;

                List<string> duplicationCheck = new List<string>();

                foreach (var item in command.Items)
                {
                    User? otherUser = null;

                    var ticketType = await _unitOfWork.SkyDiveEventTicketTypeRepository.GetByIdAsync(item.TicketTypeId);
                    if (ticketType is null)
                        throw new ManagedException("نوع بلیت مورد نظر وجود ندارد.");

                    if (item.UserCode is not null)
                    {
                        otherUser = await _unitOfWork.UserRepository.GetUserWithInclude(c => c.Code == item.UserCode);
                        if (otherUser is null)
                            throw new ManagedException("کاربری با این کد یافت نشد.");

                        if (!otherUser.UserType.AllowedTicketTypes.Any(c => c.TicketTypeId == item.TicketTypeId))
                            errors.Add($"نوع کاربری با شماره کد {item.UserCode} مجاز به رزرو بلیت های نوع '{ticketType.Title}' نیست.");

                        if (otherUser.Status != UserStatus.Active)
                            errors.Add($"وضعیت حساب کاربری با کد {item.UserCode} تایید نشده است و امکان رزرو بلیت وجود ندارد.");
                    }
                    else
                    {
                        if (!user.UserType.AllowedTicketTypes.Any(c => c.TicketTypeId == item.TicketTypeId))
                            errors.Add($"نوع کاربری شما مجاز به رزرو بلیت های نوع '{ticketType.Title}' نیست.");

                        if (user.Status != UserStatus.Active)
                            errors.Add($"وضعیت حساب کاربری شما تایید نشده است و امکان رزرو بلیت وجود ندارد.");
                    }

                    SkyDiveEventItem skyDiveEventItem = null;

                    skyDiveEvent = skyDiveEvents?
                        .FirstOrDefault(skyDiveEvent =>
                        {
                            skyDiveEventItem = skyDiveEvent.Items.FirstOrDefault(day => day.FlightLoads.Any(flight => flight.Id == item.FlightLoadId));
                            if (skyDiveEventItem is not null)
                                return true;
                            else
                                return false;
                        });

                    if (skyDiveEventItem is null)
                        throw new ManagedException("پرواز مورد نظر یافت نشد.");

                    if (skyDiveEvent is null)
                        throw new ManagedException("رویدادی پیدا نشد.");

                    if (!skyDiveEvent.IsActive || !skyDiveEvent.Status.Reservable)
                        throw new ManagedException("بلیت های این رویداد قابل رزرو نیست.");

                    var flightLoad = skyDiveEventItem.FlightLoads.FirstOrDefault(c => c.Id == item.FlightLoadId);
                    var flightLoadItem = flightLoad.FlightLoadItems.FirstOrDefault(c => c.FlightLoadType.Id == item.TicketTypeId);

                    duplicationCheck.Add((otherUser?.Code ?? user.Code).ToString() + flightLoad.Number.ToString());

                    var availableTicket = flightLoadItem.Tickets.Where(c => !c.Locked && !c.ReservedByAdmin && !c.Paid).FirstOrDefault();

                    if (availableTicket is null)
                        errors.Add($"برای پرواز شماره {flightLoad.Number} بلیت {flightLoadItem.FlightLoadType.Title} به میزان درخواستی وجود ندارد.");
                    else
                    {
                        _unitOfWork.TicketRepository.ReserveTicket(availableTicket, user);

                        flightLoadItems.Add(new Tuple<FlightLoadItem, User>(flightLoadItem, otherUser ?? user));
                    }
                }

                if (duplicationCheck.Count != duplicationCheck.Distinct().Count())
                    throw new ManagedException("هر شخص مجاز به رزرو یک بلیت در هر پرواز می‌باشد.");

                if (errors.Any())
                    throw new ManagedException(string.Join("\n", errors));

                if (_unitOfWork.ShoppingCartRepository.Include(c => c.SkyDiveEvent, c => c.Items).Any(c => c.SkyDiveEvent != skyDiveEvent && c.Items.Any()))
                    throw new ManagedException("شما سبد خرید تسویه نشده برای رویداد دیگری دارید، لطفا اقدام به حذف آیتم های سبد خرید یا تسویه آن ها نمایید.");

                await _unitOfWork.ShoppingCartRepository.ClearShoppingCartAsync(user);
                await _unitOfWork.ShoppingCartRepository
                    .AddToShoppingCart(user, flightLoadItems, skyDiveEvent!);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task CancelTicketResponse(Guid id, bool response)
        {
            var request = await _unitOfWork.AdminCartableRepository.GetFirstWithIncludeAsync(c => c.Id == id, c => c.Applicant);
            if (request is null)
                throw new ManagedException("درخواست مورد نظر یافت نشد.");

            var ticket = await _unitOfWork.TicketRepository.GetFirstWithIncludeAsync(c => c.RelatedAdminCartableRequest == request, c => c.RelatedAdminCartableRequest);
            if (ticket is null)
                throw new ManagedException("برای این بلیت درخواست کنسلی ثبت نشده است.");

            if (response)
            {
                var paidAmount = ticket.PaidAmount;

                await _unitOfWork.TicketRepository.SetAsCancelled(ticket);
                await _unitOfWork.UserRepository.AddMessage(request.Applicant, $"در خواست لغو بلیت با شماره {ticket.TicketNumber} توسط ادمین تایید شد.", "تایید لغو بلیت");

                var wallet = await _unitOfWork.WalletRepository.GetFirstWithIncludeAsync(c => c.User == request.Applicant, c => c.User);
                if (wallet is not null)
                    _unitOfWork.WalletRepository.ChangeWalletBalance(wallet, paidAmount);
            }
            else
            {
                _unitOfWork.TicketRepository.ClearCancellationRequest(ticket);
                await _unitOfWork.UserRepository.AddMessage(request.Applicant, $"در خواست لغو بلیت با شماره {ticket.TicketNumber} توسط ادمین رد شد.", "رد لغو بلیت");
            }

            _unitOfWork.AdminCartableRepository.SetAsDone(request);
            await _unitOfWork.CommitAsync();
        }

        public async Task<MemoryStream> PrintTicket(Guid id)
        {
            PersianCalendar pc = new PersianCalendar();

            var ticket = await _unitOfWork.TicketRepository.GetFirstWithIncludeAsync(c => c.Id == id, c => c.ReservedBy);
            if (ticket is null)
                throw new ManagedException("بلیت مورد نظر یافت نشد.");

            var skyDiveEvent = await _unitOfWork.SkyDiveEventRepository.GetByIdAsync(ticket.SkyDiveEventId.Value);

            return PdfHelper.TicketPdf(ticket.ReservedBy.FullName, ticket.TicketType, ticket.TicketNumber,
                skyDiveEvent.Location, $"{pc.GetYear(ticket.FlightDate.Value)}/{pc.GetMonth(ticket.FlightDate.Value)}/{pc.GetDayOfMonth(ticket.FlightDate.Value)}",
                ticket.FlightNumber.Value.ToString("000"), ticket.ReservedBy.NationalCode ?? string.Empty);
        }

        public async Task<ShoppingCartDTO> GetUserShoppingCart(Guid userId)
        {
            var settings = await _unitOfWork.SettingsRepository.FirstOrDefaultAsync(c => true);

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربر یافت نشد.");

            var shoppingCart = await _unitOfWork.ShoppingCartRepository.GetUserShoppingCart(user);
            if (shoppingCart is null || !shoppingCart.Items.Any())
                throw new ManagedException("سبد خرید شما خالی است.");

            var data = await _unitOfWork.SkyDiveEventRepository.GetExpandedSkyDiveEvent(shoppingCart.SkyDiveEvent.Id);

            var shoppingCartDto = new ShoppingCartDTO(shoppingCart.Id, shoppingCart.CreatedAt, shoppingCart.UpdatedAt)
            {
                Items = shoppingCart.Items.Select(shoppingCartItem =>
                    new ShoppingCartItemDTO(data?.FirstOrDefault(c => c.FlightLoadItem == shoppingCartItem.FlightLoadItem)?.FlightLoad.Number ?? 0,
                    shoppingCartItem.ReservedFor.Code, shoppingCartItem.FlightLoadItem.FlightLoadType.Title,
                    shoppingCart.SkyDiveEvent?.TypesAmount?.FirstOrDefault(c => c.Type == shoppingCartItem.FlightLoadItem.FlightLoadType)?.Amount ?? 0,
                    shoppingCart.SkyDiveEvent!.SubjecToVAT, data!.FirstOrDefault(c => c.FlightLoadItem == shoppingCartItem.FlightLoadItem)!.FlightLoad.Id
                    , shoppingCartItem.FlightLoadItem.FlightLoadType.Id)).ToList()
            };

            shoppingCartDto.SkyDiveEventId = shoppingCart.SkyDiveEvent.Id;

            shoppingCartDto.TotalAmount = shoppingCartDto.Items.Sum(s => s.Amount);

            shoppingCartDto.TaxAmount = shoppingCart.SkyDiveEvent.SubjecToVAT ? Math.Round((settings.VAT * shoppingCartDto.TotalAmount) / 100) : 0;

            return shoppingCartDto;
        }

        private void UnlockShoppingCartItems(User user)
        {
            var tickets = _unitOfWork.TicketRepository.Include(c => c.LockedBy).Where(c => c.LockedBy == user && !c.Paid);

            foreach (var ticket in tickets)
            {
                _unitOfWork.TicketRepository.Unlock(ticket);
            }
        }

        private async Task<double> ReservationProcess(ShoppingCart shoppingCart, User user)
        {
            var (status, errors) = await CheckTickets(user.Id);
            if (!status)
                throw new ManagedException(errors);

            var settings = await _unitOfWork.SettingsRepository.FirstOrDefaultAsync(c => true);

            int? number = null;
            double payableAmount = 0;
            foreach (var shoppingCartItem in shoppingCart.Items)
            {
                var ticket = shoppingCartItem.FlightLoadItem.Tickets.FirstOrDefault(c => !c.Paid && !c.ReservedByAdmin && (!c.Locked || c.LockedBy == user));
                if (ticket is null)
                    throw new ManagedException("بلیت مورد نظر یافت نشد.");

                var ticketAmount = shoppingCart.SkyDiveEvent!.TypesAmount.FirstOrDefault(c => c.Type == shoppingCartItem.FlightLoadItem.FlightLoadType)!.Amount;
                payableAmount += ticketAmount + (shoppingCart.SkyDiveEvent.SubjecToVAT ? Math.Round((ticketAmount * settings.VAT) / 100) : 0);

                var flightLoad = await _unitOfWork.FlightLoadRepository.GetFlightLoadByItem(shoppingCartItem.FlightLoadItem);

                _unitOfWork.TicketRepository.SetAsPaid(ticket, ticketAmount, shoppingCartItem.ReservedFor,
                    shoppingCart.SkyDiveEvent.Id, flightLoad!.Number, shoppingCartItem.FlightLoadItem.FlightLoadType.Title, flightLoad.Date, user);

                number = await _unitOfWork.TransactionRepositroy.AddTransaction(ticket.TicketNumber,
                    shoppingCart.SkyDiveEvent.Location + " کد " + shoppingCart.SkyDiveEvent.Code.ToString("000"), "", ticketAmount, TransactionType.Confirmed, user, number);
            }

            await _unitOfWork.ShoppingCartRepository.ClearShoppingCartAsync(user);
            await _unitOfWork.CommitAsync();

            return payableAmount;
        }
    }
}
