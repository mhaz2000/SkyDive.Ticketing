using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.UserValidators;

namespace SkyDiveTicketing.Application.Commands.UserCommands
{
    public class UserSecurityInformationCompletionCommand : UserCommand, ICommandBase
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public override void Validate() => new UserSecurityInformationCompletionCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
