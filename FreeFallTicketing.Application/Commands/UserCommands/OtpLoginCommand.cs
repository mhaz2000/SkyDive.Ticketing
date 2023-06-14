using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.UserValidators;

namespace SkyDiveTicketing.Application.Commands.UserCommands
{
    public class OtpLoginCommand : ICommandBase
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public void Validate() => new OtpLoginCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
