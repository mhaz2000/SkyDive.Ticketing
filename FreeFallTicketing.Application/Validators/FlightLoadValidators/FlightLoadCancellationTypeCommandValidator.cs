using FluentValidation;
using SkyDiveTicketing.Application.Commands.FlightLoadCommands;

namespace SkyDiveTicketing.Application.Validators.FlightLoadValidators
{
    public class FlightLoadCancellationTypeCommandValidator : AbstractValidator<FlightLoadCancellationTypeCommand>
    {
        public FlightLoadCancellationTypeCommandValidator()
        {
            RuleFor(c => c.FlightLoadId).NotNull().NotEmpty().WithMessage("شناسه پرواز نمی‌تواند خالی باشد.");
            RuleFor(c => c.CancellationTypes).NotNull().NotEmpty().WithMessage("کنسلی نمی‌تواند خالی باشد.");
            RuleFor(c => c.CancellationTypes == null ? 0 : c.CancellationTypes.Count()).GreaterThan(0).WithMessage("کنسلی نمی‌تواند خالی باشد.");
        }
    }
}
