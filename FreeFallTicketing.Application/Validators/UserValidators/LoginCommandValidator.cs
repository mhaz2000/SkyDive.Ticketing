using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Username).NotNull().WithMessage("نام کاربری الزامی است.");
            RuleFor(x => x.Password).NotNull().WithMessage("رمز عبور الزامی است.");
        }
    }
}