using SkyDiveTicketing.Application.Commands.Reservation;
using SkyDiveTicketing.Application.DTOs.ShoppingCartDTOs;
using SkyDiveTicketing.Application.DTOs.TicketDTOs;

namespace SkyDiveTicketing.Application.Services.ReservationServices
{
    public interface IReservationService
    {
        Task Update(ReserveCommand command, Guid userId);
        Task UnlockTickets();
        Task<(bool, string)> CheckTickets(Guid userId);
        Task<IEnumerable<MyTicketDTO>> GetUserTickets(Guid userId, TicketStatus? status = null);
        Task<bool> SetAsPaid(Guid userId);
        Task CancelTicketRequest(Guid id, Guid userId);
        Task CancelTicketResponse(Guid id, bool response);
        Task<MemoryStream> PrintTicket(Guid id);
        Task<ShoppingCartDTO> GetUserShoppingCart(Guid userId);
    }
}
