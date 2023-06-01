using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    public class OtpUserCommandValidator : AbstractValidator<OtpUserCommand>
    {
        public OtpUserCommandValidator()
        {
            RuleFor(c => c.Phone).NotEmpty().NotNull().WithMessage("شماره موبایل نمی‌تواند خالی باشد.");

        }
    }
}
