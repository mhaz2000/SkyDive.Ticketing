using FluentValidation;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;

namespace SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts
{
    public class AddSkyDiveEventFlightCommandValidator : AbstractValidator<AddSkyDiveEventFlightCommand>
    {
        public AddSkyDiveEventFlightCommandValidator()
        {
            RuleFor(c=> c.VoidableNumber).NotNull().NotEmpty().WithMessage("تعداد غیر قابل نمی‌تواند خالی باشد.");
            RuleFor(c=> c.FlightNumber).NotNull().NotEmpty().WithMessage("تعداد پرواز نمی‌تواند خالی باشد.");

            RuleFor(c => c.VoidableNumber).GreaterThan(0).WithMessage("تعداد غیر قابل رزرو نمی‌تواند منفی باشد.");
            RuleFor(c => c.FlightNumber).GreaterThan(1).WithMessage("تعداد پرواز باید بزرگتر از 0 باشد.");

            RuleFor(c => c.TickTypes.Count()).GreaterThan(0).WithMessage("حداقل یک نوع از بلیط را باید وارد کنید.");
            RuleFor(c=> c.TickTypes.Select(s=>s.TypeId).Count() - c.TickTypes.Select(s => s.TypeId).Distinct().Count()).Equal(0).WithMessage("نوع بلیط تکراری است.");
        }
    }
}
