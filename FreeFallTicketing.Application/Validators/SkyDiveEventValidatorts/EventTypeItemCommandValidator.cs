using FluentValidation;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;

namespace SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts
{
    internal class EventTypeItemCommandValidator : AbstractValidator<EventTypeItemCommand>
    {
        public EventTypeItemCommandValidator()
        {
            RuleFor(c=> c.Amount).NotNull().NotEmpty().WithMessage("مبلغ برای نوع بلیت الزامی است.");
            RuleFor(c=> c.TypeId).NotNull().NotEmpty().WithMessage("نوع بلیت الزامی است.");

            RuleFor(c => c.Amount).GreaterThan(0).WithMessage("مبلغ نمی‌تواند منفی باشد.");
        }
    }
}
