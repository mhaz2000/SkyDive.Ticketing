using SkyDiveTicketing.Application.Commands.FlightLoadCommands;
using SkyDiveTicketing.Application.DTOs.FlightLoadDTOs;

namespace SkyDiveTicketing.Application.Services.FlightLoadServices
{
    public interface IFlightLoadService
    {
        Task<Guid> Create(FlightLoadCommand command);
        IQueryable<FlightLoadDTO> GetAllFlighLoads(string search);
        Task<FlightLoadDTO> GetFlighLoad(Guid id);
        Task Remove(Guid id);
        Task Update(FlightLoadCommand command, Guid id);
    }
}
