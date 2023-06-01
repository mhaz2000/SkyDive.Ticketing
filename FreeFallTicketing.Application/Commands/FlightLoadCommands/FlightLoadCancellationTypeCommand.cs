using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.FlightLoadValidators;

namespace SkyDiveTicketing.Application.Commands.FlightLoadCommands
{
    public class FlightLoadCancellationTypeCommand : ICommandBase
    {
        public Guid FlightLoadId { get; set; }

        public IEnumerable<FlightLoadCancellationTypeDetailCommand>? CancellationTypes { get; set; }

        public void Validate() => new FlightLoadCancellationTypeCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }

    public class FlightLoadCancellationTypeDetailCommand : ICommandBase
    {
        public string Title { get; set; }
        public int HoursBeforeCancellation { get; set; }
        public float Rate { get; set; }
        public void Validate() => new FlightLoadCancellationTypeDetailCommandValidator().Validate(this).RaiseExceptionIfRequired();

    }
}
