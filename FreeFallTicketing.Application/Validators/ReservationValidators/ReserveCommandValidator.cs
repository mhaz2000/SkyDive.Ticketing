using FluentValidation;
using SkyDiveTicketing.Application.Commands.Reservation;

namespace SkyDiveTicketing.Application.Validators.ReservationValidators
{
    public class ReserveCommandValidator : AbstractValidator<ReserveCommand>
    {
        public ReserveCommandValidator()
        {
            RuleFor(c=> c.Items).NotNull().NotEmpty().WithMessage("آیتم های سبد خرید نمی‌تواند خالی باشد.");
            RuleFor(c => c.Items.Count()).GreaterThan(0).WithMessage("حداقل یه بلیت را انتخاب کنید.");
        }
    }
}
