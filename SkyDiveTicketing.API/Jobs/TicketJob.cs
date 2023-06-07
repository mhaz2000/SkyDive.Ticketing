using SkyDiveTicketing.Application.Services.ReservationServices;

namespace SkyDiveTicketing.API.Jobs
{
    public class TicketJob : ITicketJob
    {
        private readonly IReservationService _reservationService;
        public TicketJob(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public async Task CheckTicketLockTime()
        {
            await _reservationService.UnlockTickets();
        }
    }
}
