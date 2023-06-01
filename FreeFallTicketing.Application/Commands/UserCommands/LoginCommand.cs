using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.UserValidators;

namespace SkyDiveTicketing.Application.Commands.UserCommands
{
    public class LoginCommand : ICommandBase
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        public void Validate() => new LoginCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
