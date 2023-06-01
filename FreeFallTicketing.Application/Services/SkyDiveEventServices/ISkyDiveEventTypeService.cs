using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;
using SkyDiveTicketing.Application.DTOs.SkyDiveEventDTOs;

namespace SkyDiveTicketing.Application.Services.SkyDiveEventServices
{
    public interface ISkyDiveEventStatusService
    {
        Task<SkyDiveEventStatusDTO> GetStatus(Guid id);
        IEnumerable<SkyDiveEventStatusDTO> GetStatuses();
        Task Remove(Guid id);
        Task Update(SkyDiveEventStatusCommand command, Guid id);
        Task Create(SkyDiveEventStatusCommand command);
    }
}
