using FluentValidation;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts
{
    public class UpdateTicketCommandValidator : AbstractValidator<UpdateTicketCommand>
    {
        public UpdateTicketCommandValidator()
        {
            RuleFor(c=> c.Reservable).NotNull().WithMessage("قابل رزرو بودن یا نبودن الزامی است.");
            RuleFor(c=> c.Id).NotNull().WithMessage("شناسه بلیت الزامی است.");
            RuleFor(c=> c.TicketTypeId).NotNull().WithMessage("نوع بلیت الزامی است");
        }
    }
}
