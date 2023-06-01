using FluentValidation;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;

namespace SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts
{
    internal class AddEventConditionsAndTermsCommandValidator : AbstractValidator<AddEventConditionsAndTermsCommand>
    {
        public AddEventConditionsAndTermsCommandValidator()
        {
            RuleFor(c=> c.ConditionsAndTerms).NotNull().NotEmpty().WithMessage("شرایط و قوانین نمی‌تواند خالی باشد.");
        }
    }
}
