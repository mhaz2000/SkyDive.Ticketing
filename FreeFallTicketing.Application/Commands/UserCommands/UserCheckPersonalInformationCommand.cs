using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.UserValidators;

namespace SkyDiveTicketing.Application.Commands.UserCommands
{
    public class UserCheckPersonalInformationCommand : UserCommand
    {
        public bool IsConfirmed { get; set; }
        public override void Validate()
        {
            base.Validate();
            new UserCheckPersonalInformationCommandValidator().Validate(this).RaiseExceptionIfRequired();
        }
    }
}
