using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;
using System.Text.RegularExpressions;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    public class UserSecurityInformationCompletionCommandValidator : AbstractValidator<UserSecurityInformationCompletionCommand>
    {
        public UserSecurityInformationCompletionCommandValidator()
        {
            //The password must be at least 6 characters and contains both letters and numbers.
            var passwordReg = new Regex(@"^(?=.*[0-9])(?=.*[A-Za-z]).{6,}$");
            var usernameReg = new Regex(@"^(?=.{5,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$");

            RuleFor(c => c.Id).NotEmpty().NotNull().WithMessage("شناسه کاربر نمی‌تواند خالی باشد.");
            RuleFor(c => c.Username).NotEmpty().NotNull().WithMessage("نام کاربری نمی‌تواند خالی باشد.");
            RuleFor(c => c.Password).NotEmpty().NotNull().WithMessage("کلمه عبور نمی‌تواند خالی باشد.");

            RuleFor(c => c.Password).Must(pass => passwordReg.IsMatch(pass)).WithMessage("رمز باید حداقل 6 کاراکتر و شامل اعداد و حروف باشد!");
            RuleFor(c => c.Username).Must(username => passwordReg.IsMatch(username)).WithMessage("نام کاربری اشتباه است!");
        }
    }
}
