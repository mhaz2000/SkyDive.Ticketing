using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;
using SkyDiveTicketing.Application.DTOs.FlightLoadDTOs;
using SkyDiveTicketing.Application.DTOs.SkyDiveEventDTOs;
using SkyDiveTicketing.Application.DTOs.TicketDTOs;
using SkyDiveTicketing.Application.Helpers;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace SkyDiveTicketing.Application.Services.SkyDiveEventServices
{
    public class SkyDiveEventService : ISkyDiveEventService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SkyDiveEventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Create(SkyDiveEventCommand command)
        {
            var status = await _unitOfWork.SkyDiveEventStatusRepository.GetByIdAsync(command.StatusId);
            if (status is null)
                throw new ManagedException("وضعیت رویداد معتبر نیست.");

            await _unitOfWork.SkyDiveEventRepository.Create(command.Title, command.Location, command.Voidable,
                command.SubjecToVAT, command.Image, command.StartDate, command.EndDate, status);

            await _unitOfWork.CommitAsync();
        }

        public async Task<SkyDiveEventDTO> GetEvent(Guid id)
        {
            PersianCalendar pc = new PersianCalendar();
            var skyDiveEvent = _unitOfWork.SkyDiveEventRepository.FindEvents(c => c.Id == id).FirstOrDefault();
            if (skyDiveEvent is null)
                throw new ManagedException("رویداد مورد نظر یافت نشد.");

            return await Task.FromResult(new SkyDiveEventDTO(skyDiveEvent.Id, skyDiveEvent.CreatedAt, skyDiveEvent.UpdatedAt, skyDiveEvent.Title, skyDiveEvent.StartDate, skyDiveEvent.EndDate,
                skyDiveEvent.Image, skyDiveEvent.IsActive, skyDiveEvent.Items?.Sum(c => GetEmptyCapacity(c)) ?? 0, skyDiveEvent.Code.ToString("000"), skyDiveEvent.Location, skyDiveEvent.SubjecToVAT,
                skyDiveEvent.Voidable, skyDiveEvent.TermsAndConditions ?? "", skyDiveEvent.Status.Title, skyDiveEvent.Status.Reservable, skyDiveEvent.Status.Id,
                skyDiveEvent.Items.Select(item => new SkyDiveEventDayDTO($"{pc.GetYear(item.Date)}/{pc.GetMonth(item.Date)}/{pc.GetDayOfMonth(item.Date)}", item.Id))));
        }

        public IEnumerable<SkyDiveEventDTO> GetEvents(Guid? statusId, DateTime? start, DateTime? end, Guid userId)
        {
            var isAdmin = _unitOfWork.RoleRepository.GetAdminUsers().Contains(userId);

            PersianCalendar pc = new PersianCalendar();
            var events = _unitOfWork.SkyDiveEventRepository.FindEvents(c => isAdmin ? true : c.IsActive);

            if (statusId is not null)
                events = events.Where(c => c.Status.Id == statusId);

            if (start is not null)
                events = events.Where(c => c.StartDate >= start);

            if (end is not null)
                events = events.Where(c => c.EndDate <= end);

            return events.OrderByDescending(c => c.StartDate)
                .Select(skyDiveEvent => new SkyDiveEventDTO(skyDiveEvent.Id, skyDiveEvent.CreatedAt, skyDiveEvent.UpdatedAt, skyDiveEvent.Title, skyDiveEvent.StartDate, skyDiveEvent.EndDate,
                skyDiveEvent.Image, skyDiveEvent.IsActive, skyDiveEvent.Capacity, skyDiveEvent.Code.ToString("000"), skyDiveEvent.Location, skyDiveEvent.SubjecToVAT,
                skyDiveEvent.Voidable, skyDiveEvent.TermsAndConditions ?? "", skyDiveEvent.Status.Title, skyDiveEvent.Status.Reservable, skyDiveEvent.Status.Id,
                skyDiveEvent.Items.Select(item => new SkyDiveEventDayDTO($"{pc.GetYear(item.Date)}/{pc.GetMonth(item.Date)}/{pc.GetDayOfMonth(item.Date)}", item.Id))));
        }

        public async Task ToggleActivationEvent(Guid id)
        {

            var skyDiveEvent = _unitOfWork.SkyDiveEventRepository.FindEvents(c => c.Id == id).FirstOrDefault();
            if (skyDiveEvent is null)
                throw new ManagedException("رویداد مورد نظر یافت نشد.");

            if (!skyDiveEvent.IsActive)
            {
                if (!skyDiveEvent.Items.All(item => item.FlightLoads.Count() > 0))
                    throw new ManagedException("ابتدا اطلاعات مربوط به پرواز ها را تکمیل نمایید.");
            }

            _unitOfWork.SkyDiveEventRepository.ToggleActivation(skyDiveEvent);
            await _unitOfWork.CommitAsync();
        }

        public async Task Remove(Guid id)
        {
            var skyDiveEvent = _unitOfWork.SkyDiveEventRepository.FindEvents(c => c.Id == id).FirstOrDefault();
            if (skyDiveEvent is null)
                throw new ManagedException("رویداد مورد نظر یافت نشد.");

            if (skyDiveEvent.Items.Any(t => t.FlightLoads.Any()))
                throw new ManagedException("برای این رویداد، پرواز ثبت شده است و امکان حذف آن وجود ندارد.");

            _unitOfWork.SkyDiveEventRepository.Remove(skyDiveEvent);
            await _unitOfWork.CommitAsync();
        }

        public async Task Update(SkyDiveEventUpdateCommand command, Guid id)
        {
            var skyDiveEvent = await _unitOfWork.SkyDiveEventRepository.GetFirstWithIncludeAsync(c => c.Id == id, c => c.Status);
            if (skyDiveEvent is null)
                throw new ManagedException("رویداد مورد نظر یافت نشد.");

            var status = await _unitOfWork.SkyDiveEventStatusRepository.GetByIdAsync(command.StatusId);
            if (status is null)
                throw new ManagedException("وضعیت رویداد معتبر نیست.");

            var condition = await _unitOfWork.SkyDiveEventRepository.HasFlightLoad(id) && skyDiveEvent.IsActive &&
                await _unitOfWork.SkyDiveEventRepository.CheckIfCriticalDataIsChanged(skyDiveEvent, command.Title, command.Location, command.Voidable,
                command.SubjecToVAT, command.StartDate, command.EndDate);

            if (condition)
                throw new ManagedException("برای این رویداد پرواز ثبت شده است.");

            await _unitOfWork.SkyDiveEventRepository.Update(command.Title, command.Location, command.Voidable, command.SubjecToVAT,
                command.Image, command.StartDate, command.EndDate, status, skyDiveEvent);

            await _unitOfWork.CommitAsync();
        }

        public async Task AddFlight(AddSkyDiveEventFlightCommand command, Guid id)
        {
            var skyDiveEvent = _unitOfWork.SkyDiveEventRepository.FindEvents(c => c.Items.Any(t => t.Id == id)).FirstOrDefault();
            if (skyDiveEvent is null)
                throw new ManagedException("رویداد مورد نظر یافت نشد.");

            var skyDiveEventDay = skyDiveEvent.Items.FirstOrDefault(c => c.Id == id);
            if (skyDiveEvent is null)
                throw new ManagedException("روز رویداد صحیح نیست.");

            var ticketTypes = await _unitOfWork.SkyDiveEventTicketTypeRepository.GetAllAsync();

            int sum = 0;
            IDictionary<SkyDiveEventTicketType, int> typesAmount = new Dictionary<SkyDiveEventTicketType, int>();
            foreach (var item in command.TicketTypes)
            {
                var ticketType = ticketTypes.FirstOrDefault(c => c.Id == item.TypeId);
                sum += item.Qty * ticketType.Capacity;

                typesAmount.Add(ticketType, item.Qty);
            }

            await _unitOfWork.SkyDiveEventRepository.AddFlightsAsync(skyDiveEvent, skyDiveEventDay, typesAmount, command.FlightQty, command.VoidableQty, sum);

            await _unitOfWork.CommitAsync();
        }

        public string GetLastCode(Guid id)
        {
            var lastCode = _unitOfWork.SkyDiveEventRepository.OrderByDescending(c => c.Code).FirstOrDefault()?.Code;

            if (lastCode is not null)
                return (lastCode.Value + 1).ToString("000");
            else
                return "001";
        }

        public async Task<(string, IEnumerable<SkyDiveEventDaysDTO>)> GetEventDays(Guid id, Guid userId)
        {
            var skyDiveEvent = _unitOfWork.SkyDiveEventRepository.FindEvents(c => c.Id == id).FirstOrDefault();
            if (skyDiveEvent is null)
                throw new ManagedException("رویداد مورد نظر یافت نشد.");

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            return (skyDiveEvent.Title, skyDiveEvent.Items
                .Select(skyDiveEventDay => new SkyDiveEventDaysDTO(skyDiveEventDay.Id, skyDiveEventDay.CreatedAt, skyDiveEvent.UpdatedAt,
                skyDiveEventDay.Date, GetEmptyCapacity(skyDiveEventDay), skyDiveEventDay.FlightNumber, GetUserEventTicket(skyDiveEventDay, user))));
        }

        public async Task AddEventTypeFee(AddEventTypeFeeCommand command, Guid id)
        {
            var skyDiveEvent = _unitOfWork.SkyDiveEventRepository.FindEvents(c => c.Id == id).FirstOrDefault();
            if (skyDiveEvent is null)
                throw new ManagedException("رویداد مورد نظر یافت نشد.");

            var ticketTypes = _unitOfWork.SkyDiveEventTicketTypeRepository.GetAll();
            _unitOfWork.SkyDiveEventRepository.ClearTicketTypesAmount(skyDiveEvent);

            foreach (var item in command.Items)
            {
                var ticketType = ticketTypes.FirstOrDefault(c => c.Id == item.TypeId);
                if (ticketType is null)
                    throw new ManagedException("نوع بلیت یافت نشد.");

                await _unitOfWork.SkyDiveEventRepository.AddTicketTypeAmount(skyDiveEvent, ticketType, item.Amount);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task AddConditionsAndTerms(AddEventConditionsAndTermsCommand command, Guid id)
        {
            var skyDiveEvent = await _unitOfWork.SkyDiveEventRepository.GetFirstWithIncludeAsync(c => c.Id == id, c => c.Items);
            if (skyDiveEvent is null)
                throw new ManagedException("رویداد مورد نظر یافت نشد.");

            _unitOfWork.SkyDiveEventRepository.AddConditionsAndTerms(skyDiveEvent, command.ConditionsAndTerms);
            await _unitOfWork.CommitAsync();
        }

        public SkyDiveEventItemDTO GetEventDayFlights(Guid id, int pageSize, int pageIndex)
        {
            var skyDiveEvent = _unitOfWork.SkyDiveEventRepository.FindEvents(c => c.Items.Any(c => c.Id == id)).FirstOrDefault();
            if (skyDiveEvent is null)
                throw new ManagedException("رویدادی پیدا نشد.");

            var skyDiveEventItem = skyDiveEvent.Items.FirstOrDefault(c => c.Id == id)!;

            return new SkyDiveEventItemDTO(skyDiveEventItem.Id, skyDiveEventItem.CreatedAt, skyDiveEventItem.UpdatedAt, skyDiveEventItem.Date)
            {
                Flights = skyDiveEventItem.FlightLoads.OrderBy(c => c.Number).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                    .Select(flight => new FlightDTO(flight.Id, flight.CreatedAt, flight.UpdatedAt, flight.Number,
                            flight.Capacity, flight.VoidableNumber, flight.Name, flight.Status.GetHashCode()))
            };
        }

        public async Task<IEnumerable<FlightLoadItemTicketDTO>> GetFlightTickets(Guid id)
        {
            var flightLoad = await _unitOfWork.FlightLoadRepository.GetExpandedById(id);
            if (flightLoad is null)
                throw new ManagedException("روز رویداد یافت نشد.");

            return flightLoad.FlightLoadItems.SelectMany(item => item.Tickets, (item, ticket) =>
                new FlightLoadItemTicketDTO(ticket.Id, ticket.CreatedAt, ticket.UpdatedAt, ticket.TicketNumber, item.FlightLoadType.Title,
                !ticket.ReservedByAdmin, ticket.ReservedBy is null ?
                (ticket.ReservedByAdmin ? "رزرو شده (admin)" : "رزرو نشده") :
                $"رزرو شده ({ticket.ReservedBy.FullName + " " + ticket.ReservedBy.Code})", item.FlightLoadType.Id)).OrderBy(c => c.TicketNumber);
        }

        public async Task UpdateTicket(UpdateTicketCommand command)
        {
            var ticket = await _unitOfWork.TicketRepository.GetFirstWithIncludeAsync(c => c.Id == command.Id, c => c.ReservedBy);
            if (ticket is null)
                throw new ManagedException("بلیت مورد نظر یافت نشد.");

            if (ticket.ReservedBy is not null && !ticket.Locked && !ticket.ReservedByAdmin)
                throw new ManagedException("امکان ویرایش بلیت های رزور شده وجود ندارد.");

            var flightItem = await _unitOfWork.FlightLoadRepository.GetFlightItemByTicket(ticket);
            if (flightItem is null)
                throw new ManagedException("اطلاعات پرواز یافت نشد.");

            var ticketType = await _unitOfWork.SkyDiveEventTicketTypeRepository.GetByIdAsync(command.TicketTypeId);
            if (ticketType is null)
                throw new ManagedException("نوع بلیت معتبر نیست.");

            await _unitOfWork.FlightLoadRepository.UpdateFlightTicket(flightItem, ticket, ticketType, command.Reservable);
            await _unitOfWork.CommitAsync();
        }

        public async Task PublishEvent(Guid id)
        {
            var eventTypes = await _unitOfWork.SkyDiveEventTicketTypeRepository.GetAllAsync();
            var skyDiveEvent = _unitOfWork.SkyDiveEventRepository.FindEvents(c => c.Id == id).FirstOrDefault();
            if (skyDiveEvent is null)
                throw new ManagedException("رویداد مورد نظر یافت نشد.");

            if (!skyDiveEvent.Items.All(c => c.FlightLoads.Any()))
                throw new ManagedException("اطلاعات رویداد ناقص است. برای تمامی روز های رویداد پرواز ایجاد نمایید.");

            if (skyDiveEvent.TypesAmount is null || !skyDiveEvent.TypesAmount.Any() || skyDiveEvent.TypesAmount.Count() != eventTypes.Count())
                throw new ManagedException("قیمت انواع بلیت مشخص نشده است.");

            _unitOfWork.SkyDiveEventRepository.PublishEvent(skyDiveEvent);
            await _unitOfWork.CommitAsync();
        }

        public async Task<(ReservingTicketDTO reservingTicketDTO, int count)> GetEventDayTickets(Guid id, int index, int size, Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetUserWithInclude(c => c.Id == userId);
            if (user is null)
                throw new ManagedException("کاربر یافت نشد.");

            var skyDiveEvent = _unitOfWork.SkyDiveEventRepository.FindEvents(c => c.Items.Any(t => t.Id == id)).FirstOrDefault();
            if (skyDiveEvent is null) //must check if skydive is published or not
                throw new ManagedException("رویداد مورد نظر یافت نشد.");

            var skyDiveEventDay = skyDiveEvent.Items.First(c => c.Id == id);

            var dto = new ReservingTicketDTO(skyDiveEventDay.Date, skyDiveEventDay.FlightLoads.Select(flightLoad =>
                new TicketFlightDTO(flightLoad.Number, flightLoad.FlightLoadItems.Select(item =>
                    new TicketDetailDTO(item.FlightLoadType.Title, skyDiveEvent.TypesAmount.FirstOrDefault(c => c.Type.Id == item.FlightLoadType.Id)?.Amount ?? 0,
                        item.Tickets.Where(c => c.ReservedBy is null && !c.Locked && !c.ReservedByAdmin).Count(),
                        user.UserType?.AllowedTicketTypes?.Any(c => c.TicketType == item.FlightLoadType) ?? true, item.FlightLoadType.Id)),
                    flightLoad.Id)).OrderBy(c => c.FlightNumber));

            dto.Qty = dto.Flights.Sum(flight => flight.Tickets.Sum(ticket => ticket.Qty));

            int count = dto.Flights.Count();

            dto.Flights = dto.Flights.Skip((index - 1) * size).Take(size);

            return (dto, count);
        }

        private int GetEmptyCapacity(SkyDiveEventItem skyDiveEventItem)
        {
            return skyDiveEventItem.FlightLoads.Sum(flightLoad =>
            {
                var ticketsNumber = flightLoad.FlightLoadItems.Sum(item => item.Tickets.Where(c => c.ReservedBy is null && !c.Locked && !c.ReservedByAdmin).Count());
                return ticketsNumber - flightLoad.VoidableNumber;
            });
        }

        private int GetUserEventTicket(SkyDiveEventItem skyDiveEventItem, User user)
        {
            return skyDiveEventItem.FlightLoads.Sum(flightLoad =>
            {
                return flightLoad.FlightLoadItems.Sum(item => item.Tickets.Where(ticket => ticket.ReservedBy?.Id == user.Id).Count());
            });
        }

        public async Task<IEnumerable<TicketTypeAmountDTO>> GetEventTicketTypeAmounts(Guid id)
        {
            var typesAmount = await _unitOfWork.SkyDiveEventRepository.GetSkyDiveEventTicketTypesAmount(id);
            if (typesAmount is null)
                throw new ManagedException("رویداد مورد نظر یافت نشد.");

            return typesAmount.Select(s => new TicketTypeAmountDTO(s.Amount, s.Type.Id, s.Type.Title));
        }

        public async Task RemoveFlights(Guid id)
        {
            var skyDiveEvent = await _unitOfWork.SkyDiveEventRepository.GetFirstWithIncludeAsync(c => c.Items.Any(t => t.Id == id), c => c.Items);
            if (skyDiveEvent is null)
                throw new ManagedException("رویداد مورد نظر یافت نشد.");

            var skyDiveEventDay = skyDiveEvent.Items.FirstOrDefault(c => c.Id == id);
            if (skyDiveEventDay is null)
                throw new ManagedException("رویداد مورد نظر یافت نشد.");

            var flights = await _unitOfWork.SkyDiveEventRepository.GetSkyDiveEventDayFlights(id);
            if (flights is null || !flights.Any())
                throw new ManagedException("برای این رویداد پروازی ثبت نشده است.");

            if (flights.Any(c => c.FlightLoadItems.Any(t => t.Tickets.Any(y => y.Locked || y.Paid))))
                throw new ManagedException("بلیتی در پرواز ها رزرو شده است.");

            _unitOfWork.FlightLoadRepository.RemoveFlights(flights, skyDiveEventDay);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveTicket(Guid id)
        {
            var ticket = await _unitOfWork.TicketRepository.GetFirstWithIncludeAsync(c => c.Id == id, c => c.ReservedBy);
            if (ticket is null)
                throw new ManagedException("بلیت مورد نظر یافت نشد.");

            if (ticket.Paid || ticket.Locked)
                throw new ManagedException("امکان حذف بلیت های رزرو شده وجود ندارد.");

            var flightLoadItem = await _unitOfWork.FlightLoadRepository.GetFlightItemByTicket(ticket);
            if (flightLoadItem is null)
                throw new ManagedException("پرواز مرتبط با بلیت یافت نشد.");

            await _unitOfWork.FlightLoadRepository.RemoveTicket(flightLoadItem, ticket);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveFlight(Guid id)
        {
            var flight = await _unitOfWork.FlightLoadRepository.GetExpandedById(id);
            if (flight is null)
                throw new ManagedException("پرواز مورد نظر یافت نشد.");

            if (flight.FlightLoadItems.Any(t => t.Tickets.Any(y => y.Locked || y.Paid)))
                throw new ManagedException("بلیتی در این پرواز رزرو شده است.");


            var skyDiveEventDay = await _unitOfWork.SkyDiveEventItemRepository.GetFirstWithIncludeAsync(c=> c.FlightLoads.Any(t=> t.Id == id), c=> c.FlightLoads);

            _unitOfWork.FlightLoadRepository.RemoveFlight(skyDiveEventDay, flight);
            await _unitOfWork.CommitAsync();
        }

        public async Task SetFlightStatus(SetFlightStatusCommand command, Guid id)
        {
            var flight = await _unitOfWork.FlightLoadRepository.GetExpandedById(id);
            if (flight is null)
                throw new ManagedException("پرواز مورد نظر یافت نشد.");

            _unitOfWork.FlightLoadRepository.SetFlightStatus(flight, command.Status);
            await _unitOfWork.CommitAsync();
        }

        public async Task SetFlightName(SetFlightNameCommand command, Guid id)
        {
            var flight = await _unitOfWork.FlightLoadRepository.GetExpandedById(id);
            if (flight is null)
                throw new ManagedException("پرواز مورد نظر یافت نشد.");

            _unitOfWork.FlightLoadRepository.SetFlightName(flight, command.Name);
            await _unitOfWork.CommitAsync();
        }
    }
}
