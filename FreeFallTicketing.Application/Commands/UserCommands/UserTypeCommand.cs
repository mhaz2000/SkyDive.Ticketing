using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.UserValidators;

namespace SkyDiveTicketing.Application.Commands.UserCommands
{
    public class UserTypeCommand : ICommandBase
    {
        public string? Title { get; set; }
        public void Validate() => new UserTypeCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
