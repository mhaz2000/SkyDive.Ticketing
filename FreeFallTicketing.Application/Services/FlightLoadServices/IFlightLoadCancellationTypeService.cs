using SkyDiveTicketing.Application.Commands.FlightLoadCommands;
using SkyDiveTicketing.Application.DTOs.FlightLoadDTOs;

namespace SkyDiveTicketing.Application.Services.FlightLoadServices
{
    public interface IFlightLoadCancellationTypeService
    {
        FlightLoadCancellationTypeDTO GetFlightLoadCancellationType(Guid id);
        Task Remove(Guid id);
        Task Create(FlightLoadCancellationTypeCommand command);
        Task Update(FlightLoadCancellationTypeDetailCommand command, Guid id);
        Task<IEnumerable<FlightLoadCancellationTypeDTO>> GetFlightLoadCancellationRates(string search, Guid flightLoadId);
    }
}
