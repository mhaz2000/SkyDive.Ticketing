using FluentValidation;
using SkyDiveTicketing.Application.Commands.Reservation;

namespace SkyDiveTicketing.Application.Validators.ReservationValidators
{
    public class ReserveCommandValidator : AbstractValidator<ReserveCommand>
    {
        public ReserveCommandValidator()
        {
            RuleFor(c=> c.FlightLoadId).NotNull().NotEmpty().WithMessage("شناسه لود پرواز نمی‌تواند خالی باشد.");

            RuleFor(c => c.Type1SeatReservedQuantity).NotNull().NotEmpty().WithMessage("تعداد صندلی های رزروی نوع 1 نمی‌تواند خالی باشد.");
            RuleFor(c => c.Type2SeatReservedQuantity).NotNull().NotEmpty().WithMessage("تعداد صندلی های رزروی نوع 2 نمی‌تواند خالی باشد.");
            RuleFor(c => c.Type3SeatReservedQuantity).NotNull().NotEmpty().WithMessage("تعداد صندلی های رزروی نوع 3 نمی‌تواند خالی باشد.");
        }
    }
}
