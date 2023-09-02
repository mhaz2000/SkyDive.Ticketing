using FluentValidation;
using SkyDiveTicketing.Application.Commands.JumpRecordCommands;

namespace SkyDiveTicketing.Application.Validators.JumpRecordValidators
{
    internal class JumpRecordCommandValidator : AbstractValidator<JumpRecordCommand>
    {
        public JumpRecordCommandValidator()
        {
            RuleFor(c=> c.Date).NotNull().NotEmpty().WithMessage("تاریخ پرش الزامی است.");
            RuleFor(c=> c.Location).NotNull().NotEmpty().WithMessage("محل پرش الزامی است.");
            RuleFor(c=> c.Height).NotNull().NotEmpty().WithMessage("ارتفاع پرش الزامی است.");
            RuleFor(c=> c.Time).NotNull().NotEmpty().WithMessage("مدت پرش الزامی است.");

            RuleFor(c => c.Height).GreaterThan(0).WithMessage("ارتفاع پرش نمی‌تواند منفی باشد.");
        }
    }
}
