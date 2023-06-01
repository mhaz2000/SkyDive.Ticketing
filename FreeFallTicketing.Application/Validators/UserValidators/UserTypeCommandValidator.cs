using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    public class UserTypeCommandValidator : AbstractValidator<UserTypeCommand>
    {
        public UserTypeCommandValidator()
        {
            RuleFor(c=> c.Title).NotNull().NotEmpty().WithMessage("عنوان الزامی است.");
        }
    }
}
