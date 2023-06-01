using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.Reservation;
using SkyDiveTicketing.Application.DTOs;
using SkyDiveTicketing.Application.DTOs.TicketDTOs;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Application.Services.ReservationServices
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReservationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CancelTicket(Guid id)
        {
            var ticket = _unitOfWork.TicketRepository.Include(c => c.FlightLoad).FirstOrDefault(c => c.Id == id);
            if (ticket is null)
                throw new ManagedException("بلیط مورد نظر یافت نشد.");

            if (ticket.FlightLoad.CancellationTypes.Any())
            {
                var hoursLeftToFlight = Math.Abs((ticket.ReserveTime - DateTime.Now).TotalHours);

                var penaltyRate = ticket.FlightLoad.CancellationTypes.OrderBy(c => c.HoursBeforeCancellation).FirstOrDefault(c => c.HoursBeforeCancellation >= hoursLeftToFlight)?.Rate;

                var type1SeatPenaltyAmount = (ticket.Type1SeatReservedQuantity * ticket.FlightLoad.Type1SeatAmount * penaltyRate) / 100;
                var type2SeatPenaltyAmount = (ticket.Type2SeatReservedQuantity * ticket.FlightLoad.Type2SeatAmount * penaltyRate) / 100;
                var type3SeatPenaltyAmount = (ticket.Type3SeatReservedQuantity * ticket.FlightLoad.Type3SeatAmount * penaltyRate) / 100;

                var penalty = type1SeatPenaltyAmount + type2SeatPenaltyAmount + type3SeatPenaltyAmount;

                //return penalty amount to user
            }

            _unitOfWork.TicketRepository.CancelTicket(ticket);
            await _unitOfWork.CommitAsync();
        }

        public async Task<Guid> Create(ReserveCommand command)
        {
            List<string> errors = new List<string>();

            var flightLoad = await _unitOfWork.FlightLoadRepository.GetByIdAsync(command.FlightLoadId);
            if (flightLoad is null)
                throw new ManagedException("لود پرواز مورد نظر یافت نشد.");

            var previousReserves = _unitOfWork.TicketRepository.Include(c => c.FlightLoad).Where(c => c.FlightLoad.Id == command.FlightLoadId);

            if (previousReserves.Any())
            {
                var reservedType1Seat = previousReserves.Sum(c => c.Type1SeatReservedQuantity);
                var reservedType2Seat = previousReserves.Sum(c => c.Type2SeatReservedQuantity);
                var reservedType3Seat = previousReserves.Sum(c => c.Type3SeatReservedQuantity);

                if (reservedType1Seat < command.Type1SeatReservedQuantity)
                    errors.Add("گنجایش صندلی‌های نوع 1 کمتر از میزان درخواستی شما است.");

                if (reservedType2Seat < command.Type2SeatReservedQuantity)
                    errors.Add("گنجایش صندلی‌های نوع 2 کمتر از میزان درخواستی شما است.");

                if (reservedType3Seat < command.Type3SeatReservedQuantity)
                    errors.Add("گنجایش صندلی‌های نوع 3 کمتر از میزان درخواستی شما است.");
            }

            if (errors.Any())
                throw new ManagedException(string.Join("\n", errors));

            var ticketId = await _unitOfWork.TicketRepository.ReserveTicket(command.Type1SeatReservedQuantity, command.Type2SeatReservedQuantity, command.Type3SeatReservedQuantity, flightLoad);
            await _unitOfWork.CommitAsync();

            return ticketId;
        }

        public async Task Update(ReserveCommand command, Guid id)
        {
            List<string> errors = new List<string>();

            var ticket = _unitOfWork.TicketRepository.Include(c => c.FlightLoad).FirstOrDefault(c => c.Id == id);
            if (ticket is null)
                throw new ManagedException("بلیط مورد نظر یافت نشد.");

            var previousReserves = _unitOfWork.TicketRepository.Include(c => c.FlightLoad).Where(c => c.Id != id && c.FlightLoad.Id == command.FlightLoadId);

            if (previousReserves.Any())
            {
                var reservedType1Seat = previousReserves.Sum(c => c.Type1SeatReservedQuantity);
                var reservedType2Seat = previousReserves.Sum(c => c.Type2SeatReservedQuantity);
                var reservedType3Seat = previousReserves.Sum(c => c.Type3SeatReservedQuantity);

                if (reservedType1Seat < command.Type1SeatReservedQuantity)
                    errors.Add("گنجایش صندلی‌های نوع 1 کمتر از میزان درخواستی شما است.");

                if (reservedType2Seat < command.Type2SeatReservedQuantity)
                    errors.Add("گنجایش صندلی‌های نوع 2 کمتر از میزان درخواستی شما است.");

                if (reservedType3Seat < command.Type3SeatReservedQuantity)
                    errors.Add("گنجایش صندلی‌های نوع 3 کمتر از میزان درخواستی شما است.");
            }

            if (errors.Any())
                throw new ManagedException(string.Join("\n", errors));

            _unitOfWork.TicketRepository.UpdateTicket(ticket, command.Type1SeatReservedQuantity, command.Type2SeatReservedQuantity, command.Type3SeatReservedQuantity);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<TicketDTO>> GetTickets()
        {
            var tickets = await _unitOfWork.TicketRepository.GetAllAsync();

            return tickets.Select(s =>
                new TicketDTO(s.Id, s.CreatedAt, s.UpdatedAt, s.Amount, s.Type1SeatReservedQuantity, s.Type2SeatReservedQuantity, s.Type3SeatReservedQuantity,
                s.ReserveTime, s.Status, new List<PassengerDTO>()));
        }

        public async Task<TicketDTO> GetTicket(Guid id)
        {
            var ticket = await _unitOfWork.TicketRepository.GetTicketByIdAsync(id);

            var ticketDto = new TicketDTO(ticket.Id, ticket.CreatedAt, ticket.UpdatedAt, ticket.Amount, ticket.Type1SeatReservedQuantity, ticket.Type2SeatReservedQuantity, ticket.Type3SeatReservedQuantity,
                ticket.ReserveTime, ticket.Status, new List<PassengerDTO>());

            ticketDto.Passengers = ticket.Passengers.Select(p => new PassengerDTO(p.NationalCode, new CityDTO(p.City.Id, p.City.Province, p.City.State, p.City.City),
                p.Height, p.Weight, p.EmergencyContact, p.EmergencyPhone, p.MedicalDocumentFileId, p.LogBookDocumentFileId, p.AttorneyDocumentFileId, p.NationalCardDocumentFileId));

            return ticketDto;
        }

        public async Task RegisterIdentityDocuments(RegisterIdentityDocumentCommand command)
        {
            List<string> errors = new List<string>();

            var ticket = _unitOfWork.TicketRepository.Include(c => c.FlightLoad).FirstOrDefault(c => c.Id == command.TicketId);
            if (ticket is null)
                throw new ManagedException("بلیط مورد نظر یافت نشد.");

            foreach (var passengerCommand in command.Documents)
            {
                var city = await _unitOfWork.CityRepository.GetByIdAsync(passengerCommand.CityId);
                if (city is null)
                    errors.Add("مقدار شهر معتبر نیست.");

                _unitOfWork.TicketRepository.AddTicketPassenger(ticket, passengerCommand.NationalCode, city, passengerCommand.Address, passengerCommand.Height, passengerCommand.Weight,
                    passengerCommand.EmergencyContact, passengerCommand.EmergencyPhone, passengerCommand.MedicalDocumentFileId, passengerCommand.LogBookDocumentFileId,
                    passengerCommand.AttorneyDocumentFileId, passengerCommand.NationalCardDocumentFileId);
            }

            if (errors.Any())
                throw new ManagedException(string.Join("\n", errors));

            await _unitOfWork.CommitAsync();
        }
    }
}
