using FluentValidation;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;

namespace SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts
{
    internal class TicketTypeCommandValidator : AbstractValidator<TicketTypeCommand>
    {
        public TicketTypeCommandValidator()
        {
            RuleFor(c=>c.TypeId).NotNull().NotEmpty().WithMessage("نوع بلیط الزامی است.");
            RuleFor(c => c.Qty).NotNull().NotEmpty().WithMessage("تعداد بلیط الزامی است.");

            RuleFor(c => c.Qty).GreaterThan(0).WithMessage("تعداد بلیط باید بزرگتر از 0 باشد..");
        }
    }
}
