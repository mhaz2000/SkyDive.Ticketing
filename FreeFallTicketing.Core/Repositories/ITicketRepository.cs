using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        void ReserveTicket(Ticket ticket, User user);
        void SetAsCancelled(Ticket ticket);
        void SetAsPaid(Ticket ticket, double amount);
        void Unlock(Ticket ticket);
    }
}
