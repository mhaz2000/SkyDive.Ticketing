using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;
using SkyDiveTicketing.Application.DTOs.SkyDiveEventDTOs;

namespace SkyDiveTicketing.Application.Services.SkyDiveEventServices
{
    public interface ISkyDiveEventService
    {
        Task Create(SkyDiveEventCommand command);
        Task<SkyDiveEventDTO> GetEvent(Guid id);
        IEnumerable<SkyDiveEventDTO> GetEvents(Guid? statusId, DateTime? start, DateTime? end);
        Task Remove(Guid id);
        Task Update(SkyDiveEventCommand command, Guid id);
        Task ToggleActivationEvent(Guid id);
        Task AddFlight(AddSkyDiveEventFlightCommand command, Guid id);
        string GetLastCode(Guid id);
        Task<IEnumerable<SkyDiveEventDaysDTO>> GetEventDays(Guid id);
        Task AddEventTypeFee(AddEventTypeFeeCommand command, Guid id);
        Task AddConditionsAndTerms(AddEventConditionsAndTermsCommand command, Guid id);
    }
}
