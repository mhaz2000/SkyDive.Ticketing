﻿using SkyDiveTicketing.Application.Commands.Reservation;
using SkyDiveTicketing.Application.DTOs.ShoppingCartDTOs;
using SkyDiveTicketing.Application.DTOs.TicketDTOs;

namespace SkyDiveTicketing.Application.Services.ReservationServices
{
    public interface IReservationService
    {
        Task Update(ReserveCommand command, Guid userId);
        Task UnlockTickets();
        Task<(bool, string)> CheckTickets(Guid userId);
        Task<IEnumerable<MyTicketDTO>> GetUserTickets(Guid userId, string? statuses = null);
        Task<bool> SetAsPaid(Guid userId);
        Task CancelTicketRequest(Guid id, Guid userId);
        Task CancelTicketResponse(Guid id, bool response);
        Task<MemoryStream> PrintTicket(List<Guid> ids);
        Task<ShoppingCartDTO> GetUserShoppingCart(Guid userId);
        Task<bool> SetAsPaidByWallet(Guid userId);
        Task RemoveShoppingCart(Guid userId);
    }
}
