using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.WalletValidators;

namespace SkyDiveTicketing.Application.Commands.WalletCommands
{
    public class ChargeUserWalletCommand : ICommandBase
    {
        public double Amount { get; set; }
        public Guid UserId { get; set; }
        public void Validate() => new ChargeUserWalletCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
