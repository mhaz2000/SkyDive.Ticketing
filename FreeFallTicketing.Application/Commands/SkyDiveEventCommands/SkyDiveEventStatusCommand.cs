using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts;

namespace SkyDiveTicketing.Application.Commands.SkyDiveEventCommands
{
    public class SkyDiveEventStatusCommand : ICommandBase
    {
        public string? Title { get; set; }
        public bool Reservable { get; set; }
        public void Validate() => new SkyDiveEventStatusCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
