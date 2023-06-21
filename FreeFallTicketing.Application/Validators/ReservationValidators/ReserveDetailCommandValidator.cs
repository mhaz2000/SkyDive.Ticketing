using FluentValidation;
using SkyDiveTicketing.Application.Commands.Reservation;

namespace SkyDiveTicketing.Application.Validators.ReservationValidators
{
    public class ReserveDetailCommandValidator : AbstractValidator<ReserveDetailCommand>
    {
        public ReserveDetailCommandValidator()
        {
            RuleFor(c=> c.TicketTypeId).NotNull().WithMessage("نوع بلیت الزامی است.");
            RuleFor(c=> c.FlightLoadId).NotNull().WithMessage("نوع پرواز الزامی است.");
        }
    }
}
