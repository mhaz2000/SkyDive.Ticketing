using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;
using SkyDiveTicketing.Application.DTOs.SkyDiveEventDTOs;

namespace SkyDiveTicketing.Application.Services.FlightLoadServices
{
    public interface ISkyDiveEventTicketTypeService
    {
        Task<IEnumerable<SkyDiveEventTicketTypeDTO>> GetAllSkyDiveEventTicketTypes();
        Task<SkyDiveEventTicketTypeDTO> GetSkyDiveEventTicketType(Guid id);
        Task Remove(Guid id);
        Task Update(SkyDiveEventTicketTypeCommand command, Guid id);
        Task Create(SkyDiveEventTicketTypeCommand command);
    }
}
