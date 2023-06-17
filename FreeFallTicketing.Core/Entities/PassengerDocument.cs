using SkyDiveTicketing.Core.Entities.Base;
using System.ComponentModel;

namespace SkyDiveTicketing.Core.Entities
{
    
    public class LogBookDocument : BaseEntity
    {
        public LogBookDocument() : base()
        {
            Status = DocumentStatus.NotLoaded;
        }

        /// <summary>
        /// شناسه فایل
        /// </summary>
        public Guid? FileId { get; set; }

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

    public class MedicalDocument : BaseEntity
    {
        public MedicalDocument() : base()
        {
            Status = DocumentStatus.NotLoaded;
        }

        /// <summary>
        /// شناسه فایل
        /// </summary>
        public Guid? FileId { get; set; }

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

    public class AttorneyDocument : BaseEntity
    {
        public AttorneyDocument() : base()
        {
            Status = DocumentStatus.NotLoaded;
        }

        /// <summary>
        /// شناسه فایل
        /// </summary>
        public Guid? FileId { get; set; }

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

    public class NationalCardDocument : BaseEntity
    {
        public NationalCardDocument() : base()
        {
            Status = DocumentStatus.NotLoaded;
        }

        /// <summary>
        /// شناسه فایل
        /// </summary>
        public Guid? FileId { get; set; }

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
        [Description("بارگذاری نشده")]
        NotLoaded = 0,
        [Description("در انتظار تایید")]
        Pending,
        [Description("منقضی شده")]
        Expired,
        [Description("تایید شده")]
        Confirmed
    }
}
