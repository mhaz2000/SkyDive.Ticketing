using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    public class UserPersonalInformationCompletionCommandValidator : AbstractValidator<UserPersonalInformationCompletionCommand>
    {
        public UserPersonalInformationCompletionCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty().NotNull().WithMessage("شناسه کاربر نمی‌تواند خالی باشد.");
            RuleFor(c => c.BirthDate).NotEmpty().NotNull().WithMessage("تاریخ تولد نمی‌تواند خالی باشد.");
        }
    }
}
