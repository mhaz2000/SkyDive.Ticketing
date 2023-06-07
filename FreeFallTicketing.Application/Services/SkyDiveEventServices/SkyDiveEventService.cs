using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;
using SkyDiveTicketing.Application.DTOs.SkyDiveEventDTOs;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;
using System.Linq;

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
            var duplicatedCode = await _unitOfWork.SkyDiveEventRepository.AnyAsync(c => c.Code == int.Parse(command.Code));
            if (duplicatedCode)
                throw new ManagedException("کد تکراری است.");

            var status = await _unitOfWork.SkyDiveEventStatusRepository.GetByIdAsync(command.StatusId);
            if (status is null)
                throw new ManagedException("وضعیت رویداد معتبر نیست.");

            await _unitOfWork.SkyDiveEventRepository.Create(int.Parse(command.Code), command.Title, command.Location, command.Voidable,
                command.SubjecToVAT, command.Image, command.StartDate, command.EndDate, status);

            await _unitOfWork.CommitAsync();
        }

        public async Task<SkyDiveEventDTO> GetEvent(Guid id)
        {
            var skyDiveEvent = await _unitOfWork.SkyDiveEventRepository.GetFirstWithIncludeAsync(c => c.Id == id, c => c.Status);
            if (skyDiveEvent is null)
                throw new ManagedException("رویداد مورد نظر یافت نشد.");

            return new SkyDiveEventDTO(skyDiveEvent.Id, skyDiveEvent.CreatedAt, skyDiveEvent.UpdatedAt, skyDiveEvent.Title, skyDiveEvent.StartDate, skyDiveEvent.EndDate,
                skyDiveEvent.Image, skyDiveEvent.IsActive, skyDiveEvent.Capacity, skyDiveEvent.Code.ToString("000"), skyDiveEvent.Location, skyDiveEvent.SubjecToVAT,
                skyDiveEvent.Voidable, skyDiveEvent.TermsAndConditions, skyDiveEvent.Status.Title);
        }

        public IEnumerable<SkyDiveEventDTO> GetEvents(Guid? statusId, DateTime? start, DateTime? end)
        {
            var events = _unitOfWork.SkyDiveEventRepository.Include(c => c.Status);

            if (statusId is not null)
                events = events.Where(c => c.Status.Id == statusId);

            if (start is not null)
                events = events.Where(c => c.StartDate >= start);

            if (end is not null)
                events = events.Where(c => c.EndDate <= end);

            return events.Select(skyDiveEvent => new SkyDiveEventDTO(skyDiveEvent.Id, skyDiveEvent.CreatedAt, skyDiveEvent.UpdatedAt, skyDiveEvent.Title, skyDiveEvent.StartDate, skyDiveEvent.EndDate,
                skyDiveEvent.Image, skyDiveEvent.IsActive, skyDiveEvent.Capacity, skyDiveEvent.Code.ToString("000"), skyDiveEvent.Location, skyDiveEvent.SubjecToVAT,
                skyDiveEvent.Voidable, skyDiveEvent.TermsAndConditions, skyDiveEvent.Status.Title));
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
            foreach (var item in command.TickTypes)
            {
                var ticketType = ticketTypes.FirstOrDefault(c => c.Id == item.TypeId);
                sum += item.Qty * ticketType.Capacity;

                typesAmount.Add(ticketType, item.Qty);
            }

            if (sum - command.VoidableNumber != skyDiveEvent.Capacity)
                throw new ManagedException("مجموع ظرفیت وارد شده با ظرفیت پرواز ها متفاوت است.");

            _unitOfWork.SkyDiveEventRepository.AddFlights(skyDiveEventDay, typesAmount, command.FlightNumber, command.VoidableNumber);

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

        public async Task<IEnumerable<SkyDiveEventDaysDTO>> GetEventDays(Guid id, Guid userId)
        {
            var skyDiveEvent = _unitOfWork.SkyDiveEventRepository.FindEvents(c => c.Id == id).FirstOrDefault();
            if (skyDiveEvent is null)
                throw new ManagedException("رویداد مورد نظر یافت نشد.");

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربری یافت نشد.");

            return skyDiveEvent.Items
                .Select(skyDiveEventDay => new SkyDiveEventDaysDTO(skyDiveEventDay.Id, skyDiveEventDay.CreatedAt, skyDiveEvent.UpdatedAt,
                skyDiveEventDay.Date, GetEmptyCapacity(skyDiveEventDay), skyDiveEventDay.FlightNumber, GetUserEventTicker(skyDiveEventDay, user)));
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

                _unitOfWork.SkyDiveEventRepository.AddTicketTypeAmount(skyDiveEvent, ticketType, item.Amount);
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

        public SkyDiveEventItemDTO GetEventDayFlights(Guid id, int pageSize)
        {
            var skyDiveEvent = _unitOfWork.SkyDiveEventRepository.FindEvents(c => c.Items.Any(c => c.Id == id)).FirstOrDefault();
            if (skyDiveEvent is null)
                throw new ManagedException("رویدادی پیدا نشد.");

            var skyDiveEventItem = skyDiveEvent.Items.FirstOrDefault(c => c.Id == id);

            return new SkyDiveEventItemDTO(skyDiveEventItem.Id, skyDiveEventItem.CreatedAt, skyDiveEventItem.UpdatedAt, skyDiveEventItem.Date)
            {
                Flights = skyDiveEventItem.FlightLoads.OrderBy(c=> c.Number).Take(pageSize).Select(flight => new FlightDTO(flight.Id, flight.CreatedAt, flight.UpdatedAt, flight.Number)
                {
                    Tickets = flight.FlightLoadItems.Select(flightLoadItem => new TicketDTO(flightLoadItem.Id, flightLoadItem.CreatedAt, flightLoadItem.UpdatedAt,
                        flightLoadItem.FlightLoadType.Title, GetTicketTypeAmount(skyDiveEvent, flightLoadItem.FlightLoadType.Id), flightLoadItem.SeatNumber - flightLoadItem.Tickets.Count()))
                })
            };
        }

        private double GetTicketTypeAmount(SkyDiveEvent skyDiveEvent, Guid typeId)
        {
            return skyDiveEvent.TypesAmount.FirstOrDefault(c => c.Type.Id == typeId)?.Amount ?? 0;
        }

        private int GetEmptyCapacity(SkyDiveEventItem skyDiveEventItem)
        {
            return skyDiveEventItem.FlightLoads.Sum(flightLoad =>
            {
                var ticketsNumber = flightLoad.FlightLoadItems.Sum(item => item.Tickets.Count());
                return flightLoad.Capacity - ticketsNumber;
            });
        }

        private int GetUserEventTicker(SkyDiveEventItem skyDiveEventItem, User user)
        {
            return skyDiveEventItem.FlightLoads.Sum(flightLoad =>
            {
                return flightLoad.FlightLoadItems.Sum(item => item.Tickets.Where(ticket => ticket.ReservedBy.Id == user.Id).Count());
            });
        }

    }
}
