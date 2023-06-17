using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.UserValidators;

namespace SkyDiveTicketing.Application.Commands.UserCommands
{
    public class AssignTicketTypeCommand : ICommandBase
    {
        public Guid UserTypeId { get; set; }

        public IEnumerable<Guid> TicketTypes { get; set; }

        public void Validate() => new AssignTicketTypeCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
