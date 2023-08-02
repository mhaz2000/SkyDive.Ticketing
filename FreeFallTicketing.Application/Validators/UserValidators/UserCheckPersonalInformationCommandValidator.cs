using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    public class UserCheckPersonalInformationCommandValidator : AbstractValidator<UserCheckPersonalInformationCommand>
    {
        public UserCheckPersonalInformationCommandValidator()
        {
            RuleFor(c=> c.IsConfirmed).NotNull().WithMessage("تایید یا عدم تایید کاربر مشخص نشده است.");
        }
    }
}
