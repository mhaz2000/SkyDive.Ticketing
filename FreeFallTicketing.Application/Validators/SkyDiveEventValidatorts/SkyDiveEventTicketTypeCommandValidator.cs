using FluentValidation;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;

namespace SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts
{
    internal class SkyDiveEventTicketTypeCommandValidator : AbstractValidator<SkyDiveEventTicketTypeCommand>
    {
        public SkyDiveEventTicketTypeCommandValidator()
        {
            RuleFor(c => c.Title).NotNull().NotEmpty().WithMessage("عنوان نمی‌تواند خالی باشد.");
            RuleFor(c => c.Capacity).NotNull().NotEmpty().WithMessage("ظرفیت نمی‌تواند خالی باشد.");

            RuleFor(c => c.Capacity).GreaterThan(0).WithMessage("مقدار ظرفیت باید بزرگتر از 0 باشد.");
        }
    }
}
