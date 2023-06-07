using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IFlightLoadRepository : IRepository<FlightLoad>
    {
        Task<FlightLoadItem?> GetFlightItemByTicket(Ticket ticket);
    }
}
