using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;
using SkyDiveTicketing.Application.DTOs.FlightLoadDTOs;
using SkyDiveTicketing.Application.DTOs.SkyDiveEventDTOs;
using SkyDiveTicketing.Application.DTOs.TicketDTOs;
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

            return new SkyDiveEventDTO(skyDiveEvent.Id, skyDiveEvent.CreatedAt, skyDiveEvent.UpdatedAt, skyDiveEvent.Title, skyDiveEvent.StartDate, skyDiveEvent.EndDate,
                skyDiveEvent.Image, skyDiveEvent.IsActive, skyDiveEvent.Items?.Sum(c => c.FlightLoads?.Sum(t => t.Capacity) ?? 0) ?? 0, skyDiveEvent.Code.ToString("000"), skyDiveEvent.Location, skyDiveEvent.SubjecToVAT,
                skyDiveEvent.Voidable, skyDiveEvent.TermsAndConditions ?? "", skyDiveEvent.Status.Title, skyDiveEvent.Status.Reservable, skyDiveEvent.Status.Id,
                skyDiveEvent.Items.Select(item => new SkyDiveEventDayDTO($"{pc.GetYear(item.Date)}/{pc.GetMonth(item.Date)}/{pc.GetDayOfMonth(item.Date)}", item.Id)));
        }

        public IEnumerable<SkyDiveEventDTO> GetEvents(Guid? statusId, DateTime? start, DateTime? end)
        {
            PersianCalendar pc = new PersianCalendar();
            var events = _unitOfWork.SkyDiveEventRepository.FindEvents();

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

        public async Task Update(SkyDiveEventCommand command, Guid id)
        {
            var skyDiveEvent = await _unitOfWork.SkyDiveEventRepository.GetFirstWithIncludeAsync(c => c.Id == id, c => c.Status);
            if (skyDiveEvent is null)
                throw new ManagedException("رویداد مورد نظر یافت نشد.");

            var status = await _unitOfWork.SkyDiveEventStatusRepository.GetByIdAsync(command.StatusId);
            if (status is null)
                throw new ManagedException("وضعیت رویداد معتبر نیست.");

            _unitOfWork.SkyDiveEventRepository.Update(command.Title, command.Location, command.Voidable, command.SubjecToVAT,
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

            await _unitOfWork.SkyDiveEventRepository.AddFlightsAsync(skyDiveEvent, skyDiveEventDay, typesAmount, command.FlightQty, command.VoidableQty, sum + command.VoidableQty);

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

            var skyDiveEventItem = skyDiveEvent.Items.FirstOrDefault(c => c.Id == id);

            return new SkyDiveEventItemDTO(skyDiveEventItem.Id, skyDiveEventItem.CreatedAt, skyDiveEventItem.UpdatedAt, skyDiveEventItem.Date)
            {
                Flights = skyDiveEventItem.FlightLoads.OrderBy(c => c.Number).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                    .Select(flight => new FlightDTO(flight.Id, flight.CreatedAt, flight.UpdatedAt, flight.Number, flight.Capacity, flight.VoidableNumber))
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
                $"رزرو شده ({ticket.ReservedBy.FullName + " " + ticket.ReservedBy.Code})")).OrderBy(c => c.TicketNumber);
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
            var skyDiveEvent = _unitOfWork.SkyDiveEventRepository.FindEvents(c => c.Id == id).FirstOrDefault();
            if (skyDiveEvent is null)
                throw new ManagedException("رویداد مورد نظر یافت نشد.");

            if (skyDiveEvent.Items.All(c => c.FlightLoads.Any()))
                _unitOfWork.SkyDiveEventRepository.PublishEvent(skyDiveEvent);
            else
                throw new ManagedException("اطلاعات رویداد ناقص است. برای تمامی روز های رویداد پرواز ایجاد نمایید..");

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
                        user.UserType?.AllowedTicketTypes?.Any(c=> c.TicketType == item.FlightLoadType) ?? true, item.FlightLoadType.Id)),
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
                var ticketsNumber = flightLoad.FlightLoadItems.Sum(item => item.Tickets.Where(c => c.ReservedBy is null && !c.Locked).Count());
                return flightLoad.Capacity - ticketsNumber;
            });
        }

        private int GetUserEventTicket(SkyDiveEventItem skyDiveEventItem, User user)
        {
            return skyDiveEventItem.FlightLoads.Sum(flightLoad =>
            {
                return flightLoad.FlightLoadItems.Sum(item => item.Tickets.Where(ticket => ticket.ReservedBy?.Id == user.Id).Count());
            });
        }
    }
}
