using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts;
using SkyDiveTicketing.Core.Entities;

namespace SkyDiveTicketing.Application.Commands.SkyDiveEventCommands
{
    public class SetFlightStatusCommand : ICommandBase
    {
        public FlightStatus Status { get; set; }
        public void Validate() => new SetFlightStatusCommandValidator().Validate(this).RaiseExceptionIfRequired();

    }
}