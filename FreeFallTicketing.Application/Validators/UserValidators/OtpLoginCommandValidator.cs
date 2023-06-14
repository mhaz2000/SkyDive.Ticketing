using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    internal class OtpLoginCommandValidator : AbstractValidator<OtpLoginCommand>
    {
        public OtpLoginCommandValidator()
        {
            RuleFor(c => c.Id).NotNull().WithMessage("شناسه کاربر نمی‌تواند خالی باشد.");
            RuleFor(c => c.Code).NotNull().WithMessage("کد نمی‌تواند خالی باشد.");
        }
    }
}
