using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts;

namespace SkyDiveTicketing.Application.Commands.SkyDiveEventCommands
{
    public class SkyDiveEventTicketTypeCommand : ICommandBase
    {
        public string Title { get; set; }
        public int Capacity { get; set; }

        public void Validate() => new SkyDiveEventTicketTypeCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
