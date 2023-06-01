using FluentValidation;
using SkyDiveTicketing.Application.Commands.FlightLoadCommands;

namespace SkyDiveTicketing.Application.Validators.FlightLoadValidators
{
    public class FlightLoadCancellationTypeDetailCommandValidator : AbstractValidator<FlightLoadCancellationTypeDetailCommand>
    {
        public FlightLoadCancellationTypeDetailCommandValidator()
        {
            RuleFor(c=> c.Title).NotEmpty().NotNull().WithMessage("عنوان کنسلی نمی‌تواند خالی باشد.");
            RuleFor(c=> c.Rate).NotEmpty().NotNull().WithMessage("نرخ کنسلی نمی‌تواند خالی باشد.");
            RuleFor(c=> c.HoursBeforeCancellation).NotEmpty().NotNull().WithMessage("زمان قبل از کنسلی نمی‌تواند خالی باشد.");
        }
    }
}
