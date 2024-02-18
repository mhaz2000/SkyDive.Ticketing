using FluentValidation;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;

namespace SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts
{
    internal class SetFlightStatusCommandValidator : AbstractValidator<SetFlightStatusCommand>
    {
        public SetFlightStatusCommandValidator()
        {
        }
    }
}
