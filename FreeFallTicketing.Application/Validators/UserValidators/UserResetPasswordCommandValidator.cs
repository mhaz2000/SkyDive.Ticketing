using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;
using System.Text.RegularExpressions;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    public class UserResetPasswordCommandValidator : AbstractValidator<UserResetPasswordCommand>
    {
        public UserResetPasswordCommandValidator()
        {
            var passwordReg = new Regex(@"^(?=.*[0-9])(?=.*[A-Za-z]).{6,}$");

            RuleFor(c => c.Id).NotEmpty().NotNull().WithMessage("شناسه کاربر نمی‌تواند خالی باشد.");
            RuleFor(c => c.Password).NotEmpty().NotNull().WithMessage("کلمه عبور نمی‌تواند خالی باشد.");

            RuleFor(c => c.Password).Must(pass => passwordReg.IsMatch(pass)).WithMessage("رمز باید حداقل 6 کاراکتر و شامل اعداد و حروف باشد!");
        }
    }
}
