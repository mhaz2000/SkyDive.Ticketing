using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts;

namespace SkyDiveTicketing.Application.Commands.SkyDiveEventCommands
{
    public class UpdateTicketCommand : ICommandBase
    {
        public Guid Id { get; set; }
        public Guid TicketTypeId { get; set; }
        public bool Reservable { get; set; }

        public void Validate() => new UpdateTicketCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
