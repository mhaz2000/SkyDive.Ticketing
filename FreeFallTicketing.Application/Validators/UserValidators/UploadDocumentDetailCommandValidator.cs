using FluentValidation;
using SkyDiveTicketing.Application.Commands.UserCommands;

namespace SkyDiveTicketing.Application.Validators.UserValidators
{
    public class UploadDocumentDetailCommandValidator : AbstractValidator<UploadDocumentDetailCommand>
    {
        public UploadDocumentDetailCommandValidator()
        {
            RuleFor(c => c.FileId).NotNull().NotEmpty().WithMessage("شناسه فایل نمی‌تواند خالی باشد.");

            RuleFor(c => c.ExpirationDate).Must(date => date > DateTime.Now).When(date => date is not null).WithMessage("تاریخ انقضا معتبر نمی‌باشد.");
        }
    }
}
