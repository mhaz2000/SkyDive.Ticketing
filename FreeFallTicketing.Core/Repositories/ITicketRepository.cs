using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<Ticket> GetTicketByIdAsync(Guid id);

        Task<Guid> ReserveTicket(int type1SeatReserveNumber, int type2SeatReserveNumber, int type3SeatReserveNumber, FlightLoad flightLoad);

        void UpdateTicket(Ticket ticket, int type1SeatReserveNumber, int type2SeatReserveNumber, int type3SeatReserveNumber);

        void CancelTicket(Ticket ticket);

        void AddTicketPassenger(Ticket ticket, string nationalCode, DefaultCity city, string address, float height, float weight, string emergencyContact, string emergencyPhone,
            Guid medicalDocumentFileId, Guid logBookDocumentFileId, Guid attorneyDocumentFileId, Guid nationalCardDocumentFileId);
    }
}
