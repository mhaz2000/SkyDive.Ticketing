using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.UserValidators;

namespace SkyDiveTicketing.Application.Commands.UserCommands
{
    public class OtpRequestConfirmationCommand : ICommandBase
    {
        public string Phone { get; set; }
        public string Code { get; set; }

        public void Validate() => new OtpRequestConfirmationCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
