using FluentValidation;
using SkyDiveTicketing.Application.Commands.Reservation;

namespace SkyDiveTicketing.Application.Validators.ReservationValidators
{
    public class ReserveCommandValidator : AbstractValidator<ReserveCommand>
    {
        public ReserveCommandValidator()
        {

        }
    }
}
