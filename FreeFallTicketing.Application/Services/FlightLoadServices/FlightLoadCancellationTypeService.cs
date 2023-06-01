using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.FlightLoadCommands;
using SkyDiveTicketing.Application.DTOs.FlightLoadDTOs;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Application.Services.FlightLoadServices
{
    public class FlightLoadCancellationTypeService : IFlightLoadCancellationTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FlightLoadCancellationTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Create(FlightLoadCancellationTypeCommand command)
        {
            var flightLoad = await _unitOfWork.FlightLoadRepository.GetByIdAsync(command.FlightLoadId);
            if (flightLoad is null)
                throw new ManagedException("لود پرواز مورد نظر یافت نشد.");

            _unitOfWork.FlightLoadCancellationTypeRepository
                .UpdateFlightLoadCancellationType(flightLoad, command.CancellationTypes.ToDictionary(s => s.HoursBeforeCancellation, s => s.Rate));

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<FlightLoadCancellationTypeDTO>> GetFlightLoadCancellationRates(string search, Guid flightLoadId)
        {
            var flightLoad = await _unitOfWork.FlightLoadRepository.GetByIdAsync(flightLoadId);
            if (flightLoad is null)
                throw new ManagedException("لود پرواز یافت نشد.");

            return flightLoad.CancellationTypes.Select(s => new FlightLoadCancellationTypeDTO(s.Id, s.CreatedAt, s.UpdatedAt, flightLoadId, s.HoursBeforeCancellation, s.Rate));
        }

        public FlightLoadCancellationTypeDTO GetFlightLoadCancellationType(Guid id)
        {
            var cancellationType = _unitOfWork.FlightLoadCancellationTypeRepository.Include(c => c.FlightLoad).FirstOrDefault(c => c.Id == id);
            if (cancellationType is null)
                throw new ManagedException("نرخ کنسلی یافت نشد.");

            return new FlightLoadCancellationTypeDTO(cancellationType.Id, cancellationType.CreatedAt, cancellationType.UpdatedAt,
                cancellationType.FlightLoad.Id, cancellationType.HoursBeforeCancellation, cancellationType.Rate);
        }

        public Task Remove(Guid id)
        {
            var cancellationType = _unitOfWork.FlightLoadCancellationTypeRepository.Include(c => c.FlightLoad).FirstOrDefault(c => c.Id == id);
            if (cancellationType is null)
                throw new ManagedException("نرخ کنسلی یافت نشد.");

            _unitOfWork.FlightLoadRepository.RemoveFlightLoadCancellationType(cancellationType.FlightLoad, cancellationType);
            _unitOfWork.FlightLoadCancellationTypeRepository.Remove(cancellationType);

            return Task.CompletedTask;
        }

        public async Task Update(FlightLoadCancellationTypeDetailCommand command, Guid id)
        {
            var cancellationType = await _unitOfWork.FlightLoadCancellationTypeRepository.GetByIdAsync(id);
            if (cancellationType is null)
                throw new ManagedException("نرخ کنسلی یافت نشد.");

            _unitOfWork.FlightLoadCancellationTypeRepository.UpdateFlightLoadCancellationType(cancellationType, command.HoursBeforeCancellation, command.Rate);
            await _unitOfWork.CommitAsync();
        }
    }
}
