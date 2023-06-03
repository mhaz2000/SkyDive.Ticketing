using FluentValidation;
using SkyDiveTicketing.Application.Commands.Reservation;

namespace SkyDiveTicketing.Application.Validators.ReservationValidators
{
    public class ReserveDetailCommandValidator : AbstractValidator<ReserveDetailCommand>
    {
        public ReserveDetailCommandValidator()
        {
            RuleFor(c=> c.TicketTypeId).NotNull().NotEmpty().WithMessage("نوع بلیت الزامی است.");
            RuleFor(c=> c.FlightLoadId).NotNull().NotEmpty().WithMessage("نوع پرواز الزامی است.");
            RuleFor(c=> c.Qty).NotNull().NotEmpty().WithMessage("تعداد بلیت الزامی است.");

            RuleFor(c => c.Qty).GreaterThan(0).WithMessage("تعداد بلیت باید بیشتر از 0 باشد.");
        }
    }
}
