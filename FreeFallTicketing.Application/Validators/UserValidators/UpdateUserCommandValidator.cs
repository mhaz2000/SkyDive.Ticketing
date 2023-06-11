using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(c=> c.Height).GreaterThan(0).When(c=> c.Height is not null).WithMessage("قد نمی‌تواند منفی باشد.");
            RuleFor(c=> c.Weight).GreaterThan(0).When(c=> c.Weight is not null).WithMessage("وزن نمی‌تواند منفی باشد.");
            RuleFor(c => c.BirthDate).GreaterThan(DateTime.Now).When(c => c.BirthDate is not null).WithMessage("تاریخ تولد نمی‌تواند بزرگتر از تاریخ امروز باشد.");
        }
    }
}
