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
            RuleFor(c => (DateTime.Now - c.BirthDate).Days / 365).LessThan(90).WithMessage("تاریخ تولد صحیح نیست.")
                .GreaterThanOrEqualTo(12)
                .WithMessage("تاریخ تولد صحیح نیست.");

            RuleFor(c => c.Weight).LessThan(200).WithMessage("وزن صحیح نیست.")
                .GreaterThanOrEqualTo(15).When(c => c.Weight is not null)
                .WithMessage("وزن صحیح نیست.");

            RuleFor(c => c.Height).LessThanOrEqualTo(230).WithMessage("قد صحیح نیست.")
                .GreaterThanOrEqualTo(50).When(c => c.Weight is not null)
                .WithMessage("قد صحیح نیست.");

            RuleFor(c => c.NationalCode).Must(nationalCode => NationalCodeHelper.IsValidNationalCode(nationalCode))
                .When(info => !string.IsNullOrEmpty(info.NationalCode)).WithMessage("فرمت کد ملی صحیح نمی‌باشد.");
        }
    }
}
