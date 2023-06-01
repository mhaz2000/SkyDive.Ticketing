using FluentValidation;
using SkyDiveTicketing.Application.Commands.Reservation;

namespace SkyDiveTicketing.Application.Validators.ReservationValidators
{
    public class RegisterPassengerIdentityDocumentCommandValidator : AbstractValidator<RegisterPassengerIdentityDocumentCommand>
    {
        public RegisterPassengerIdentityDocumentCommandValidator()
        {
            RuleFor(c=> c.AttorneyDocumentFileId).NotNull().NotEmpty().WithMessage("وکالت نامه محضری اجباری است.");
            RuleFor(c=> c.MedicalDocumentFileId).NotNull().NotEmpty().WithMessage("سند پزشکی اجباری است.");
            RuleFor(c=> c.EmergencyPhone).NotNull().NotEmpty().WithMessage("شماره موبایل اضطراری اجباری است.");
            RuleFor(c=> c.Weight).NotNull().NotEmpty().WithMessage("فد اجباری است.");
            RuleFor(c=> c.Height).NotNull().NotEmpty().WithMessage("وزن اجباری است.");
            RuleFor(c=> c.EmergencyContact).NotNull().NotEmpty().WithMessage("نام شخص برای تماس اضطراری اجباری است.");
            RuleFor(c=> c.LogBookDocumentFileId).NotNull().NotEmpty().WithMessage("آخرین صفحه لاگ بوک اجباری است.");
            RuleFor(c=> c.NationalCardDocumentFileId).NotNull().NotEmpty().WithMessage("تصویر کارت ملی اجباری است.");
            RuleFor(c=> c.NationalCode).NotNull().NotEmpty().WithMessage("کد ملی اجباری است.");
        }
    }
}
