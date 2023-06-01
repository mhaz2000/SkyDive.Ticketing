using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    public class UserCheckPersonalInformationCommandValidator : AbstractValidator<UserCheckPersonalInformationCommand>
    {
        public UserCheckPersonalInformationCommandValidator()
        {
            RuleFor(c=> c.IsConfirmed).NotNull().NotEmpty().WithMessage("تایید یا عدم تایید مدارک بارگذاری شده توسط کاربر مشخص نشده است.");
        }
    }
}
