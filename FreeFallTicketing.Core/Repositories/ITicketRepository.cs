using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        void ClearCancellationRequest(Ticket ticket);
        void ReserveTicket(Ticket ticket, User user);
        Task SetAsCancelled(Ticket ticket);
        void SetAsPaid(Ticket ticket, double amount, User user, Guid skyDiveEventId, int flightNumber, string ticketType, DateTime flightDate, User paidBy);
        void Unlock(Ticket ticket);
    }
}
