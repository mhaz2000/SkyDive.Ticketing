using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    internal class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(DataContext context) : base(context)
        {
        }


        public Ticket AddTicket(FlightLoadItem flightLoadItem, User user, int flightNumber, SkyDiveEvent skyDiveEvent)
        {
            var lastNumber = skyDiveEvent.Items.SelectMany(s => s.FlightLoads, (skyDiveEventItem, flightLoad) => flightLoad)?
                .SelectMany(c => c.FlightLoadItems, (flightLoad, flightLoadItem) => flightLoadItem)?
                .SelectMany(c => c.Tickets, (item, ticket) => ticket)?.OrderByDescending(c => c.TicketNumber)?.FirstOrDefault()?.TicketNumber;

            var counter = string.IsNullOrEmpty(lastNumber) ? int.Parse(lastNumber.Substring(lastNumber.Count() - 4)) : 1;

            var number = skyDiveEvent.Code.ToString("000") + flightNumber.ToString("000") + counter.ToString("0000");

            var ticket = new Ticket(number, skyDiveEvent.Voidable, user, false);
            flightLoadItem.Tickets.Add(ticket);

            return ticket;
        }

        public void ClearUserTicket(User user)
        {
            var userTickets = Context.Tickets.Include(c => c.ReservedBy).Where(x => x.ReservedBy == user);
            var flightLoadItems = Context.FlightLoadItems.Include(c => c.Tickets);

            foreach (var ticket in userTickets)
            {
                var flightLoadItem = flightLoadItems.FirstOrDefault(c => c.Tickets.Contains(ticket));
                flightLoadItem.Tickets.Remove(ticket);
            }
        }

        public void SetAsCancelled(Ticket ticket)
        {
            ticket.SetAsCancelled();
        }

        public void SetAsPaid(Ticket ticket)
        {
            ticket.SetAsPaid();
        }

        public void Unlock(Ticket ticket)
        {
            ticket.SetAsUnLock();
        }
    }
}
