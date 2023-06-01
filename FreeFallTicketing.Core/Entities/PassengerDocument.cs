using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class PassengerDocument : BaseEntity
    {
        public PassengerDocument() : base()
        {
            Status = DocumentStatus.NotLoaded;
        }

        /// <summary>
        /// شناسه فایل
        /// </summary>
        public Guid FileId { get; set; }

        /// <summary>
        /// وضعیت مدرک
        /// </summary>
        public DocumentStatus Status { get; private set; }

        /// <summary>
        /// تاریخ انقضای مدرک
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        public void Upload(Guid fileId, DateTime? expirationDate)
        {
            FileId = fileId;
            ExpirationDate = expirationDate;
            Status = DocumentStatus.Pending;
        }

        public void SetStatus(DocumentStatus status) => Status = status;
    }

    public enum DocumentStatus
    {
        NotLoaded = 0,
        Pending,
        Expired,
        Confirmed
    }
}
