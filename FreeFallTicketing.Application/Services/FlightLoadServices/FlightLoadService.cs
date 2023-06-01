using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.FlightLoadCommands;
using SkyDiveTicketing.Application.DTOs.FlightLoadDTOs;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Application.Services.FlightLoadServices
{
    public class FlightLoadService : IFlightLoadService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FlightLoadService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> Create(FlightLoadCommand command)
        {
            if (await _unitOfWork.FlightLoadRepository.AnyAsync(c => c.Number == command.Number))
                throw new ManagedException("شماره پرواز تکراری است.");

            var id = await _unitOfWork.FlightLoadRepository.
                CreateFlightLoad(command.Number, command.Date, command.Type1SeatNumber, command.Type2SeatNumber, command.Type3SeatNumber,
                command.Type1SeatNumber, command.Type2SeatAmount, command.Type3SeatAmount);

            await _unitOfWork.CommitAsync();

            return id;
        }

        public IQueryable<FlightLoadDTO> GetAllFlighLoads(string search)
        {
            search = search ?? string.Empty;

            return _unitOfWork.FlightLoadRepository.Where(c => c.Number.Contains(search))
                .Select(c => new FlightLoadDTO(c.Id, c.CreatedAt, c.UpdatedAt, c.Number, c.Date, c.Type1SeatNumber, c.Type2SeatNumber, c.Type3SeatNumber));
        }

        public async Task<FlightLoadDTO> GetFlighLoad(Guid id)
        {
            var flightLoad = await _unitOfWork.FlightLoadRepository.GetByIdAsync(id);
            if (flightLoad is null)
                throw new ManagedException("لود پرواز یافت نشد.");

            return new FlightLoadDTO(flightLoad.Id, flightLoad.CreatedAt, flightLoad.UpdatedAt,
                flightLoad.Number, flightLoad.Date, flightLoad.Type1SeatNumber, flightLoad.Type2SeatNumber, flightLoad.Type3SeatNumber);
        }

        public async Task Remove(Guid id)
        {
            var flightLoad = await _unitOfWork.FlightLoadRepository.GetByIdAsync(id);
            if (flightLoad is null)
                throw new ManagedException("لود پرواز یافت نشد.");

            _unitOfWork.FlightLoadRepository.Remove(flightLoad);
            await _unitOfWork.CommitAsync();
        }

        public async Task Update(FlightLoadCommand command, Guid id)
        {
            var flightLoad = await _unitOfWork.FlightLoadRepository.GetByIdAsync(id);
            if (flightLoad is null)
                throw new ManagedException("لود پرواز یافت نشد.");

            _unitOfWork.FlightLoadRepository
                .UpdateFlightLoad(command.Number, command.Date, command.Type1SeatNumber, command.Type2SeatNumber, command.Type3SeatNumber,
                command.Type1SeatAmount, command.Type2SeatAmount, command.Type3SeatAmount, flightLoad);

            await _unitOfWork.CommitAsync();
        }
    }
}
