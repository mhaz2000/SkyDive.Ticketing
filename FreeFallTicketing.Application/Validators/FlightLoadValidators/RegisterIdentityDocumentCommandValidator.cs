using FluentValidation;
using SkyDiveTicketing.Application.Commands.Reservation;

namespace SkyDiveTicketing.Application.Validators.FlightLoadValidators
{
    public class RegisterIdentityDocumentCommandValidator : AbstractValidator<RegisterIdentityDocumentCommand>
    {
        public RegisterIdentityDocumentCommandValidator()
        {
            RuleFor(c => c.TicketId).NotNull().NotEmpty().WithMessage("شناسه بلیط اجباری است.");
            RuleFor(c => c.Documents.Count()).GreaterThan(0).WithMessage("اطلاعات مسافران اجباری است.");
        }
    }
}
