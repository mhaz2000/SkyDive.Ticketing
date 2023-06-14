using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    public class UserPersonalInformationCompletionCommandValidator : AbstractValidator<UserPersonalInformationCompletionCommand>
    {
        public UserPersonalInformationCompletionCommandValidator()
        {
            RuleFor(c => c.Id).NotNull().WithMessage("شناسه کاربر نمی‌تواند خالی باشد.");

            RuleFor(c => c.BirthDate).NotNull().WithMessage("تاریخ تولد نمی‌تواند خالی باشد.");
        }
    }
}
