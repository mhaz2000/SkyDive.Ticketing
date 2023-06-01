using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.FlightLoadValidators;

namespace SkyDiveTicketing.Application.Commands.FlightLoadCommands
{
    public class FlightLoadCommand : ICommandBase
    {
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public int Type1SeatNumber { get; set; }
        public int Type2SeatNumber { get; set; }
        public int Type3SeatNumber { get; set; }

        public double Type1SeatAmount { get; set; }
        public double Type2SeatAmount { get; set; }
        public double Type3SeatAmount { get; set; }

        public void Validate() => new FlightLoadCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
