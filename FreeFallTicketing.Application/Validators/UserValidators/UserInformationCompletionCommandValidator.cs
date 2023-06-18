using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using SkyDiveTicketing.Application.Commands.UserCommands;
using SkyDiveTicketing.Application.Helpers;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    public class UserPersonalInformationCompletionCommandValidator : AbstractValidator<UserPersonalInformationCompletionCommand>
    {
        public UserPersonalInformationCompletionCommandValidator()
        {
            RuleFor(c => c.BirthDate).NotNull().WithMessage("تاریخ تولد نمی‌تواند خالی باشد.");
            RuleFor(c => c.NationalCode).Must(nationalCode => NationalCodeHelper.IsValidNationalCode(nationalCode))
                .When(info => !string.IsNullOrEmpty(info.NationalCode)).WithMessage("فرمت کد ملی صحیح نمی‌باشد.");
        }
    }
}
