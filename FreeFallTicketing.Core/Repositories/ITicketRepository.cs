using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Ticket AddTicket(FlightLoadItem flightLoadItem, User user, int flightNumber, SkyDiveEvent skyDiveEvent);
        void ClearUserTicket(User user);
        void SetAsCancelled(Ticket ticket);
        void SetAsPaid(Ticket ticket);
        void Unlock(Ticket ticket);
    }
}
