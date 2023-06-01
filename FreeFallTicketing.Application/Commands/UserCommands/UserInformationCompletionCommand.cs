using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.UserValidators;

namespace SkyDiveTicketing.Application.Commands.UserCommands
{
    public class UserPersonalInformationCompletionCommand : UserCommand, ICommandBase
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? NationalCode { get; set; }
        public DateTime BirthDate { get; set; }

        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? EmergencyContact { get; set; }
        public string? EmergencyPhone { get; set; }
        public float? Height { get; set; }
        public float? Weight { get; set; }
        public Guid? CityId { get; set; }

        /// <summary>
        /// فایل سند پزشکی
        /// </summary>
        public UploadDocumentDetailCommand? MedicalDocument { get; set; }

        /// <summary>
        /// سند لاگ بوک
        /// </summary>
        public UploadDocumentDetailCommand? LogBookDocument { get; set; }

        /// <summary>
        /// سند وکالت نامه محضری
        /// </summary>
        public UploadDocumentDetailCommand? AttorneyDocument { get; set; }

        /// <summary>
        /// فایل کارت ملی
        /// </summary>
        public UploadDocumentDetailCommand? NationalCardDocument { get; set; }

        public override void Validate() => new UserPersonalInformationCompletionCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }

    public class UploadDocumentDetailCommand : ICommandBase
    {
        public Guid FileId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public virtual void Validate() => new UploadDocumentDetailCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
