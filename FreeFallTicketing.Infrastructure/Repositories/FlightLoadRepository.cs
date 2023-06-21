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
            return await Context.FlightLoadItems
                .Include(c=> c.FlightLoadType)
                .Include(c => c.Tickets).FirstOrDefaultAsync(c => c.Tickets.Contains(ticket));
        }

        public async Task<FlightLoad?> GetExpandedById(Guid id)
        {
            return await Context.FlightLoads
                .Include(c => c.FlightLoadItems).ThenInclude(c => c.FlightLoadType)
                .Include(c => c.FlightLoadItems).ThenInclude(c => c.Tickets).ThenInclude(c => c.ReservedBy)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateFlightTicket(FlightLoadItem flightItem, Ticket ticket, SkyDiveEventTicketType ticketType, bool reservable)
        {
            var flightLoad = await Context.FlightLoads
                .Include(c => c.FlightLoadItems).ThenInclude(c => c.Tickets)
                .Include(c => c.FlightLoadItems).ThenInclude(c => c.FlightLoadType)
                .FirstOrDefaultAsync(c => c.FlightLoadItems.Any(item => item.Id == flightItem.Id));

            flightItem.SeatNumber--;
            flightItem.Tickets.Remove(ticket);

            flightLoad.Capacity -= flightItem.FlightLoadType.Capacity;

            Context.Tickets.Remove(ticket);

            var newTicket = new Ticket(ticket.TicketNumber, ticket.Voidable, null, !reservable);

            await Context.Tickets.AddAsync(newTicket);

            var item = flightLoad.FlightLoadItems.FirstOrDefault(c => c.FlightLoadType.Id == ticketType.Id);
            if (item is not null)
            {
                item.SeatNumber++;
                item.Tickets.Add(newTicket);
            }
            else
            {
                var newFlightItem = new FlightLoadItem(ticketType, 1);
                newFlightItem.Tickets.Add(newTicket);

                await Context.FlightLoadItems.AddAsync(newFlightItem);

                flightLoad.FlightLoadItems.Add(newFlightItem);
            }

            flightLoad.Capacity += ticketType.Capacity;
        }

        public Task<FlightLoad?> GetFlightLoadByItem(FlightLoadItem flightLoadItem)
        {
            return Context.FlightLoads.Include(c=> c.FlightLoadItems).FirstOrDefaultAsync(c=>c.FlightLoadItems.Contains(flightLoadItem));
        }
    }
}
