using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    public class UserCommandValidator : AbstractValidator<UserCommand>
    {
        public UserCommandValidator()
        {
            RuleFor(x=> x.Id).NotNull().WithMessage("شناسه کاربر نمی‌تواند خالی باشد.");
        }
    }
}
