using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    internal class OtpRequestConfirmationCommandValidator : AbstractValidator<OtpRequestConfirmationCommand>
    {
        public OtpRequestConfirmationCommandValidator()
        {
            RuleFor(c=>c.Phone).NotEmpty().WithMessage("شماره موبایل الزامی است.");
            RuleFor(c=>c.Code).NotEmpty().WithMessage("کد الزامی است.");
        }
    }
}
