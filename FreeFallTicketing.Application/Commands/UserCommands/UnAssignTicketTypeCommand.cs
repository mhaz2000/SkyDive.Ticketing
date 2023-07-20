using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.UserValidators;

namespace SkyDiveTicketing.Application.Commands.UserCommands
{
    public class UnAssignTicketTypeCommand : ICommandBase
    {
        public Guid UserTypeId { get; set; }

        public Guid TicketTypeId { get; set; }

        public void Validate() => new UnAssignTicketTypeCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
