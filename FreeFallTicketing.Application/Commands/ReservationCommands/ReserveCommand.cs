using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.ReservationValidators;

namespace SkyDiveTicketing.Application.Commands.Reservation
{
    public class ReserveCommand : ICommandBase
    {
        public Guid FlightLoadId { get; set; }
        public int Type1SeatReservedQuantity { get; set; }
        public int Type2SeatReservedQuantity { get; set; }
        public int Type3SeatReservedQuantity { get; set; }
        public void Validate() => new ReserveCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
