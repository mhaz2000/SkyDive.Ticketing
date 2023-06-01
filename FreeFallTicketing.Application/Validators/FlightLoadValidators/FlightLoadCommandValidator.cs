using FluentValidation;
using SkyDiveTicketing.Application.Commands.FlightLoadCommands;

namespace SkyDiveTicketing.Application.Validators.FlightLoadValidators
{
    public class FlightLoadCommandValidator : AbstractValidator<FlightLoadCommand>
    {
        public FlightLoadCommandValidator()
        {
            RuleFor(c => c.Number).NotEmpty().NotNull().WithMessage("شماره پرواز نمی‌تواند خالی باشد.");
            RuleFor(c => c.Date).NotEmpty().NotNull().WithMessage("تاریخ پرواز نمی‌تواند خالی باشد.");
            RuleFor(c => c.Type1SeatNumber).NotEmpty().NotNull().WithMessage("تعداد صندلی های نوع 1 نمی‌تواند خالی باشد.");
            RuleFor(c => c.Type2SeatNumber).NotEmpty().NotNull().WithMessage("تعداد صندلی های نوع 2 نمی‌تواند خالی باشد.");
            RuleFor(c => c.Type3SeatNumber).NotEmpty().NotNull().WithMessage("تعداد صندلی های نوع 3 نمی‌تواند خالی باشد.");

            RuleFor(c => c.Type1SeatAmount).NotEmpty().NotNull().When(c => c.Type1SeatNumber > 0).WithMessage("قیمت صندلی‌های نوع 1 نمی‌تواند خالی باشد.");
            RuleFor(c => c.Type2SeatAmount).NotEmpty().NotNull().When(c => c.Type2SeatNumber > 0).WithMessage("قیمت صندلی‌های نوع 2 نمی‌تواند خالی باشد.");
            RuleFor(c => c.Type3SeatAmount).NotEmpty().NotNull().When(c => c.Type3SeatNumber > 0).WithMessage("قیمت صندلی‌های نوع 3 نمی‌تواند خالی باشد.");

            RuleFor(c => c.Type1SeatNumber + c.Type2SeatNumber + c.Type3SeatNumber)
                .GreaterThan(0).WithMessage("مجموع تعداد صندلی‌های پرواز باید بیش تر از صفر باشد.");
        }
    }
}
