using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    public class OtpUserConfirmationCommandValidator : AbstractValidator<OtpUserConfirmationCommand>
    {
        public OtpUserConfirmationCommandValidator()
        {
            RuleFor(c => c.Phone).NotEmpty().NotNull().WithMessage("شماره موبایل نمی‌تواند خالی باشد.");
            RuleFor(c => c.Code).NotEmpty().NotNull().WithMessage("کد نمی‌تواند خالی باشد.");
        }
    }
}
