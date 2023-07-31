using SkyDiveTicketing.Application.Commands.Base;

namespace SkyDiveTicketing.Application.Commands.UserCommands
{
    public class AdminUploadUserDocumentCommand : ICommandBase
    {
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

        public void Validate()
        {
        }
    }
}
