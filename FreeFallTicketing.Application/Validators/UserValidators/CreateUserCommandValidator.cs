using FluentValidation;
using SkyDiveTicketing.Application.Commands;
using SkyDiveTicketing.Application.Commands.UserCommands;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(c => c.Phone).NotEmpty().NotNull().WithMessage("شماره موبایل نمی‌تواند خالی باشد.");
        }
    }
}
