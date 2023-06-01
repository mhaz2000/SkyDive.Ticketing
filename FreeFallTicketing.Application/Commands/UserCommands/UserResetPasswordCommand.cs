using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.UserValidators;

namespace SkyDiveTicketing.Application.Commands.UserCommands
{
    public class UserResetPasswordCommand : UserCommand, ICommandBase
    {
        public string? Password { get; set; }

        public override void Validate() => new UserResetPasswordCommandValidator().Validate(this).RaiseExceptionIfRequired();

    }
}
