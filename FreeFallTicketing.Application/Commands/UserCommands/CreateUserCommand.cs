using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.UserValidators;

namespace SkyDiveTicketing.Application.Commands.UserCommands
{
    public class CreateUserCommand : ICommandBase
    {
        public string? Phone { get; set; }

        public void Validate() => new CreateUserCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
