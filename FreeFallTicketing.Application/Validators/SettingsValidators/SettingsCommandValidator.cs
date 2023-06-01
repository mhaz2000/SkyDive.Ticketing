using FluentValidation;
using SkyDiveTicketing.Application.Commands.SettingsCommands;


namespace SkyDiveTicketing.Application.Validators.SettingsValidators
{
    public class SettingsCommandValidator : AbstractValidator<SettingsCommand>
    {
        public SettingsCommandValidator()
        {

        }
    }
}
