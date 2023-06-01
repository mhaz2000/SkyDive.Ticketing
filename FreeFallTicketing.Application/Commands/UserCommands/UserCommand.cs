using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.UserValidators;

namespace SkyDiveTicketing.Application.Commands.UserCommands
{
    public class UserCommand : ICommandBase
    {
        public string? Id { get; set; }
        public virtual void Validate() => new UserCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
