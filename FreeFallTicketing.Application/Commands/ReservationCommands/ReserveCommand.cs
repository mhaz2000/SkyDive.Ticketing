using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.ReservationValidators;

namespace SkyDiveTicketing.Application.Commands.Reservation
{
    public class ReserveCommand : ICommandBase
    {
        public List<ReserveDetailCommand> Items { get; set; }
        public void Validate()
        {
            Items.ForEach(x => x.Validate());
            new ReserveCommandValidator().Validate(this).RaiseExceptionIfRequired();
        }
    }

    public class ReserveDetailCommand : ICommandBase
    {
        public Guid FlightLoadId { get; set; }
        public Guid TicketTypeId { get; set; }
        public int? UserCode { get; set; }

        public void Validate() => new ReserveDetailCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}