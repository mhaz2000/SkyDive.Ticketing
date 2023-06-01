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

        public void AddTicketPassenger(Ticket ticket, string nationalCode, DefaultCity city, string address, float height, float weight,
            string emergencyContact, string emergencyPhone, Guid medicalDocumentFileId, Guid logBookDocumentFileId, Guid attorneyDocumentFileId, Guid nationalCardDocumentFileId)
        {
            ticket.AddPassenger(new Passenger(nationalCode, city, address, height, weight, emergencyContact, emergencyPhone));
        }

        public void CancelTicket(Ticket ticket)
        {
            ticket.SetAsDeleted();
        }

        public async Task<Guid> ReserveTicket(int type1SeatReserveNumber, int type2SeatReserveNumber, int type3SeatReserveNumber, FlightLoad flightLoad)
        {
            var entity = new Ticket(flightLoad, type1SeatReserveNumber, type2SeatReserveNumber, type3SeatReserveNumber);
            await Context.Tickets.AddAsync(entity);

            return entity.Id;
        }

        public void UpdateTicket(Ticket ticket, int type1SeatReserveNumber, int type2SeatReserveNumber, int type3SeatReserveNumber)
        {
            ticket.Type1SeatReservedQuantity = type1SeatReserveNumber;
            ticket.Type2SeatReservedQuantity = type2SeatReserveNumber;
            ticket.Type3SeatReservedQuantity = type3SeatReserveNumber;

            ticket.UpdateAmount();
        }

        public async Task<Ticket> GetTicketByIdAsync(Guid id)
        {
            return await Context.Tickets.Include(c => c.Passengers).ThenInclude(c => c.City).FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
