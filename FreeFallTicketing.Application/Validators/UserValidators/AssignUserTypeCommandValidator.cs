using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    internal class AssignUserTypeCommandValidator : AbstractValidator<AssignUserTypeCommand>
    {
        public AssignUserTypeCommandValidator()
        {
            RuleFor(c=> c.UserTypeId).NotNull().NotEmpty().WithMessage("نوع کاربر نمی‌تواند خالی باشد.");
        }
    }
}
