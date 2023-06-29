using FluentValidation;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;

namespace SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts
{
    public class AddSkyDiveEventFlightCommandValidator : AbstractValidator<AddSkyDiveEventFlightCommand>
    {
        public AddSkyDiveEventFlightCommandValidator()
        {
            RuleFor(c=> c.VoidableQty).NotNull().WithMessage("تعداد غیر قابل نمی‌تواند خالی باشد.");
            RuleFor(c=> c.FlightQty).NotNull().WithMessage("تعداد پرواز نمی‌تواند خالی باشد.");

            RuleFor(c => c.VoidableQty).GreaterThanOrEqualTo(0).WithMessage("تعداد غیر قابل رزرو نمی‌تواند منفی باشد.");
            RuleFor(c => c.FlightQty).GreaterThan(1).WithMessage("تعداد پرواز باید بزرگتر از 0 باشد.");

            RuleFor(c => c.TicketTypes.Count()).GreaterThan(0).WithMessage("حداقل یک نوع از بلیت را باید وارد کنید.");
            RuleFor(c=> c.TicketTypes.Select(s=>s.TypeId).Count() - c.TicketTypes.Select(s => s.TypeId).Distinct().Count()).Equal(0).WithMessage("نوع بلیت تکراری است.");
        }
    }
}
