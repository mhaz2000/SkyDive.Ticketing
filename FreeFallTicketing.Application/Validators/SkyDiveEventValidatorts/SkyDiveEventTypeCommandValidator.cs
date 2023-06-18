using FluentValidation;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;

namespace SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts
{
    public class SkyDiveEventStatusCommandValidator : AbstractValidator<SkyDiveEventStatusCommand>
    {
        public SkyDiveEventStatusCommandValidator()
        {
            RuleFor(c=> c.Title).NotNull().NotEmpty().WithMessage("عنوان نمی‌تواند خالی باشد.");
            RuleFor(c=> c.Reservable).NotNull().WithMessage("وضعیت رزرو باید مشخص گردد.");
        }
    }
}
