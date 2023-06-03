using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.UserValidators;

namespace SkyDiveTicketing.Application.Commands.UserCommands
{
    public class AssignUserTypeCommand : UserCommand
    {
        public Guid UserTypeId { get; set; }
        public override void Validate()
        {
            base.Validate();
            new AssignUserTypeCommandValidator().Validate(this).RaiseExceptionIfRequired();
        }
    }
}
