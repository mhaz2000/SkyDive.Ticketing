using FluentValidation;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;

namespace SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts
{
    internal class AddEventTypeFeeCommandValidator : AbstractValidator<AddEventTypeFeeCommand>
    {
        public AddEventTypeFeeCommandValidator()
        {
            RuleFor(c => c.SkyEventTypeId).NotNull().NotEmpty().WithMessage("رویداد الزامی است.");
            RuleFor(c => c.Items).NotNull().NotEmpty().WithMessage("حداقل قیمت یک نوع بلیت باید مشخص شده باشد.");
            RuleFor(c=> c.Items.Count()).GreaterThan(0).WithMessage("حداقل قیمت یک نوع بلیت باید مشخص شده باشد.");

            RuleFor(c=> c.Items.Select(c => c.TypeId).Count() - c.Items.Distinct().Select(c => c.TypeId).Count()).Equal(0).WithMessage("امکان ثبت نوع تکراری وجود ندارد.");
        }
    }
}
