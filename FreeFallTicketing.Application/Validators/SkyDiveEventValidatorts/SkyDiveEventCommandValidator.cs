using FluentValidation;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;

namespace SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts
{
    public class SkyDiveEventCommandValidator : AbstractValidator<SkyDiveEventCommand>
    {
        public SkyDiveEventCommandValidator()
        {
            int res = 0;
            RuleFor(c => c.StartDate).NotNull().NotEmpty().WithMessage("تاریخ شروع رویداد نمی‌تواند خالی باشد.");
            RuleFor(c => c.EndDate).NotNull().NotEmpty().WithMessage("تاریخ پایان رویداد نمی‌تواند خالی باشد.");
            RuleFor(c => c.Title).NotNull().NotEmpty().WithMessage("عنوان رویداد نمی‌تواند خالی باشد.");
            RuleFor(c => c.Location).NotNull().NotEmpty().WithMessage("محل رویداد نمی‌تواند خالی باشد.");
            RuleFor(c => c.StartDate).GreaterThan(DateTime.Now).WithMessage("تاریخ شروع معتبر نمی‌باشد.");
            RuleFor(c => c.EndDate).GreaterThan(DateTime.Now).WithMessage("تاریخ پایان معتبر نمی‌باشد.");
            RuleFor(c => c.Voidable).NotNull().NotEmpty().WithMessage("فیلد قابلیت نمی‌تواند خالی باشد.");
            RuleFor(c => c.SubjecToVAT).NotNull().NotEmpty().WithMessage("فیلد ارزش افزوده نمی‌تواند خالی باشد.");
            RuleFor(c => c.SubjecToVAT).NotNull().NotEmpty().WithMessage("وضعیت نمی‌تواند خالی باشد.");

            RuleFor(c => int.TryParse(c.Code, out res)).Equal(true).WithMessage("کد معتبر نیست.");


            RuleFor(c => c.EndDate).GreaterThan(t => t.StartDate).WithMessage("تاریخ شروع باید بعد از تاریخ پایان باشد.");
        }
    }
}
