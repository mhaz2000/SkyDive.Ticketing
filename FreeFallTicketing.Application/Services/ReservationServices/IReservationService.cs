using SkyDiveTicketing.Application.Commands.Reservation;
using SkyDiveTicketing.Application.DTOs.TicketDTOs;

namespace SkyDiveTicketing.Application.Services.ReservationServices
{
    public interface IReservationService
    {
        Task CancelTicket(Guid id);
        Task<Guid> Create(ReserveCommand command);
        Task RegisterIdentityDocuments(RegisterIdentityDocumentCommand command);
        Task Update(ReserveCommand command, Guid id);
        Task<IEnumerable<TicketDTO>> GetTickets();
        Task<TicketDTO> GetTicket(Guid id);
    }
}
