using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;
using SkyDiveTicketing.Application.DTOs.SkyDiveEventDTOs;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Application.Services.FlightLoadServices
{
    public class SkyDiveEventTicketTypeService : ISkyDiveEventTicketTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SkyDiveEventTicketTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Create(SkyDiveEventTicketTypeCommand command)
        {
            var duplicatedTitle = await _unitOfWork.SkyDiveEventTicketTypeRepository.AnyAsync(c=> c.Title == command.Title);
            if (duplicatedTitle)
                throw new ManagedException("عنوان تکراری است.");

            await _unitOfWork.SkyDiveEventTicketTypeRepository.Create(command.Title, command.Capacity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<SkyDiveEventTicketTypeDTO>> GetAllSkyDiveEventTicketTypes()
        {
            return (await _unitOfWork.SkyDiveEventTicketTypeRepository.GetAllAsync())
                .Select(skyDiveEventTicketType => 
                new SkyDiveEventTicketTypeDTO(skyDiveEventTicketType.Id, skyDiveEventTicketType.CreatedAt, skyDiveEventTicketType.UpdatedAt, skyDiveEventTicketType.Title, skyDiveEventTicketType.Capacity));
        }

        public async Task<SkyDiveEventTicketTypeDTO> GetSkyDiveEventTicketType(Guid id)
        {
            var skyDiveEventTicketType = await _unitOfWork.SkyDiveEventTicketTypeRepository.GetByIdAsync(id);
            if (skyDiveEventTicketType is null)
                throw new ManagedException("نوع بلیت یافت نشد.");

            return new SkyDiveEventTicketTypeDTO(skyDiveEventTicketType.Id, skyDiveEventTicketType.CreatedAt, skyDiveEventTicketType.UpdatedAt, skyDiveEventTicketType.Title, skyDiveEventTicketType.Capacity);
        }
        public async Task Remove(Guid id)
        {
            var skyDiveEventTicketType = await _unitOfWork.SkyDiveEventTicketTypeRepository.GetByIdAsync(id);
            if (skyDiveEventTicketType is null)
                throw new ManagedException("نوع بلیت یافت نشد.");

            //check if used
            await _unitOfWork.CommitAsync();
        }

        public async Task Update(SkyDiveEventTicketTypeCommand command, Guid id)
        {
            var skyDiveEventTicketType = await _unitOfWork.SkyDiveEventTicketTypeRepository.GetByIdAsync(id);
            if (skyDiveEventTicketType is null)
                throw new ManagedException("نوع بلیت یافت نشد.");

            //check if used
            await _unitOfWork.CommitAsync();
        }
    }
}
