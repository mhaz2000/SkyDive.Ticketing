using SkyDiveTicketing.Application.Commands.Reservation;
using SkyDiveTicketing.Application.DTOs.TicketDTOs;

namespace SkyDiveTicketing.Application.Services.ReservationServices
{
    public interface IReservationService
    {
        Task Update(ReserveCommand command, Guid userId);
        Task UnlockTickets();
        Task<(bool, string)> CheckTickets(Guid userId);
        Task<IEnumerable<MyTicketDTO>> GetUserTickets(Guid userId);
        Task<bool> SetAsPaid(Guid userId);
        Task CancelTicketRequest(Guid id, Guid userId);
        Task CancelTicketResponse(Guid id, bool response);
        MemoryStream PrintTicket(Guid id);
    }
}
