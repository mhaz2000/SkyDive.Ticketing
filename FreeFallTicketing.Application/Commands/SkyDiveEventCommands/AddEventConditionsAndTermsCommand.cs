using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts;

namespace SkyDiveTicketing.Application.Commands.SkyDiveEventCommands
{
    public class AddEventConditionsAndTermsCommand : ICommandBase
    {
        public string ConditionsAndTerms { get; set; }
        public void Validate() => new AddEventConditionsAndTermsCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
