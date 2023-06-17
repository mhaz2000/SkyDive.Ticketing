using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    public class AssignTicketTypeCommandValidator : AbstractValidator<AssignTicketTypeCommand>
    {
        public AssignTicketTypeCommandValidator()
        {
            RuleFor(c=> c.UserTypeId).NotNull().WithMessage("نوع کاربری الزامی است.");
            RuleFor(c=> c.TicketTypes).NotNull().WithMessage("حداقل یک نوع بلیط انتخاب نمایید.");
            RuleFor(c=> c.TicketTypes.Count()).GreaterThan(0).WithMessage("حداقل یک نوع بلیط انتخاب نمایید.");
        }

    }
}
