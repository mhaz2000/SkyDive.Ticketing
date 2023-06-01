using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.SettingsValidators;
using SkyDiveTicketing.Core.Entities;

namespace SkyDiveTicketing.Application.Commands.SettingsCommands
{
    public class SettingsCommand : ICommandBase
    {
        public string? TermsAndConditionsUrl { get; set; }

        public ICollection<UserStatusInfoCommand>? UserStatusInfo { get; set; }

        public void Validate() => new SettingsCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }

    public class UserStatusInfoCommand
    {
        public UserStatus Status { get; set; }
        public string? Description { get; set; }
    }
}
