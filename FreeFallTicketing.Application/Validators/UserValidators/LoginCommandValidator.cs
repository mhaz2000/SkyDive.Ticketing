using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Username).NotEmpty().NotNull().WithMessage("نام کاربری الزامی است.");
        }
    }
}
