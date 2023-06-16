using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.UserValidators;

namespace SkyDiveTicketing.Application.Commands.UserCommands
{
    public class OtpUserCommand : ICommandBase
    {
        public string Username { get; set; }

        public void Validate() => new OtpUserCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
