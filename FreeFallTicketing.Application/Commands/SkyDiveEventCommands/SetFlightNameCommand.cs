using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts;

namespace SkyDiveTicketing.Application.Commands.SkyDiveEventCommands
{
    public class SetFlightNameCommand : ICommandBase
    {
        public string Name { get; set; }
        public void Validate() => new SetFlightNameCommandValidator().Validate(this).RaiseExceptionIfRequired();

    }
}