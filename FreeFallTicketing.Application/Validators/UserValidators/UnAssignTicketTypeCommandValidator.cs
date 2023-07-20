using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    public class UnAssignTicketTypeCommandValidator : AbstractValidator<UnAssignTicketTypeCommand>
    {
        public UnAssignTicketTypeCommandValidator()
        {
            RuleFor(c => c.TicketTypeId).NotNull().WithMessage("نوع بلیت نمی‌تواند خالی باشد.");
            RuleFor(c => c.UserTypeId).NotNull().WithMessage("نوع کاربر نمی‌تواند خالی باشد.");
        }
    }
}
