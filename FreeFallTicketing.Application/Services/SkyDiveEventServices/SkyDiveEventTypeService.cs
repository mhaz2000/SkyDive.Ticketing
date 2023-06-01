using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;
using SkyDiveTicketing.Application.DTOs.SkyDiveEventDTOs;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Application.Services.SkyDiveEventServices
{
    public class SkyDiveEventStatusService : ISkyDiveEventStatusService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SkyDiveEventStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Create(SkyDiveEventStatusCommand command)
        {
            var duplicatedStatus = await _unitOfWork.SkyDiveEventStatusRepository.AnyAsync(c => c.Title == command.Title);
            if (duplicatedStatus)
                throw new ManagedException("عنوان وضعیت تکراری است.");

            await _unitOfWork.SkyDiveEventStatusRepository.Create(command.Title);
            await _unitOfWork.CommitAsync();
        }

        public async Task<SkyDiveEventStatusDTO> GetStatus(Guid id)
        {
            var status = await _unitOfWork.SkyDiveEventStatusRepository.GetByIdAsync(id);
            if (status is null)
                throw new ManagedException("وضعیت رویداد یافت نشد.");

            return new SkyDiveEventStatusDTO(status.Id, status.CreatedAt, status.UpdatedAt, status.Title);
        }

        public IEnumerable<SkyDiveEventStatusDTO> GetStatuses()
        {
            return _unitOfWork.SkyDiveEventStatusRepository.GetAll()
                .Select(status => new SkyDiveEventStatusDTO(status.Id, status.CreatedAt, status.UpdatedAt, status.Title));
        }

        public async Task Remove(Guid id)
        {
            var status = await _unitOfWork.SkyDiveEventStatusRepository.GetByIdAsync(id);
            if (status is null)
                throw new ManagedException("وضعیت رویداد یافت نشد.");

            _unitOfWork.SkyDiveEventStatusRepository.Remove(status);
            await _unitOfWork.CommitAsync();
        }

        public async Task Update(SkyDiveEventStatusCommand command, Guid id)
        {
            var status = await _unitOfWork.SkyDiveEventStatusRepository.GetByIdAsync(id);
            if (status is null)
                throw new ManagedException("وضعیت رویداد یافت نشد.");

            _unitOfWork.SkyDiveEventStatusRepository.Update(command.Title, status);
            await _unitOfWork.CommitAsync();
        }
    }
}
