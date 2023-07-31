using FluentValidation;
using SkyDiveTicketing.Application.Commands.WalletCommands;

namespace SkyDiveTicketing.Application.Validators.WalletValidators
{
    public class ChargeUserWalletCommandValidator : AbstractValidator<ChargeUserWalletCommand>
    {
        public ChargeUserWalletCommandValidator()
        {
            RuleFor(c => c.Amount).NotNull().WithMessage("مبلغ الزامی است.");
            RuleFor(c => c.UserId).NotNull().WithMessage("شناسه کاربر الزامی است.");
        }
    }
}
