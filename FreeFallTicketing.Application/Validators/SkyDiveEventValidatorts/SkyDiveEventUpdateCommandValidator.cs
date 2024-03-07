using FluentValidation;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;

namespace SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts
{
    public class SkyDiveEventUpdateCommandValidator : AbstractValidator<SkyDiveEventUpdateCommand>
    {
        public SkyDiveEventUpdateCommandValidator()
        {
            RuleFor(c => c.StartDate).NotNull().WithMessage("تاریخ شروع رویداد نمی‌تواند خالی باشد.");
            RuleFor(c => c.EndDate).NotNull().WithMessage("تاریخ پایان رویداد نمی‌تواند خالی باشد.");
            RuleFor(c => c.Title).NotNull().WithMessage("عنوان رویداد نمی‌تواند خالی باشد.");
            RuleFor(c => c.Location).NotNull().WithMessage("محل رویداد نمی‌تواند خالی باشد.");
            RuleFor(c => c.Voidable).NotNull().WithMessage("فیلد قابلیت نمی‌تواند خالی باشد.");
            RuleFor(c => c.SubjecToVAT).NotNull().WithMessage("فیلد ارزش افزوده نمی‌تواند خالی باشد.");
            RuleFor(c => c.SubjecToVAT).NotNull().WithMessage("وضعیت نمی‌تواند خالی باشد.");

            RuleFor(c => c.EndDate).GreaterThanOrEqualTo(t => t.StartDate).WithMessage("تاریخ شروع نمی‌تواند قبل از تاریخ پایان باشد.");
        }
    }
}
