using Microsoft.EntityFrameworkCore;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class FlightLoadRepository : Repository<FlightLoad>, IFlightLoadRepository
    {
        public FlightLoadRepository(DataContext context) : base(context)
        {
        }

        public async Task<FlightLoadItem?> GetFlightItemByTicket(Ticket ticket)
        {
            return await Context.FlightLoadItems.Include(c=>c.Tickets).FirstOrDefaultAsync(c=> c.Tickets.Contains(ticket));
        }
    }
}
