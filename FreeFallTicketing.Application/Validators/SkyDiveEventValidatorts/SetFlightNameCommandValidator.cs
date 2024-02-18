using FluentValidation;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;

namespace SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts
{
    internal class SetFlightNameCommandValidator : AbstractValidator<SetFlightNameCommand>
    {
        public SetFlightNameCommandValidator()
        {
            RuleFor(c => c.Name).NotNull().WithMessage("نام اجباری است.");
            RuleFor(c => c.Name).NotEmpty().When(c=> c.Name is not null).WithMessage("نام اجباری است.");
        }
    }
}
