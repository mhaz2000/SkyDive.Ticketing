using SkyDiveTicketing.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDiveTicketing.Application.DTOs.UserDTOs
{
    public class UserDocumentsDTO : BaseDTO<Guid>
    {
        public UserDocumentsDTO(Guid id, DateTime createdAt, DateTime updatedAt) : base(id, createdAt, updatedAt)
        {
        }

        /// <summary>
        /// فایل سند پزشکی
        /// </summary>
        public UserDocumentDetailDTO? MedicalDocument { get; set; }

        /// <summary>
        /// سند لاگ بوک
        /// </summary>
        public UserDocumentDetailDTO? LogBookDocument { get; set; }

        /// <summary>
        /// سند وکالت نامه محضری
        /// </summary>
        public UserDocumentDetailDTO? AttorneyDocument { get; set; }

        /// <summary>
        /// فایل کارت ملی
        /// </summary>
        public UserDocumentDetailDTO? NationalCardDocument { get; set; }
    }

    public class UserDocumentDetailDTO : BaseDTO<Guid>
    {
        public UserDocumentDetailDTO(Guid id, DateTime createdAt, DateTime updatedAt, Guid? fileId, DateTime? expirationDate, string statusDisplay, DocumentStatus status) : base(id, createdAt, updatedAt)
        {
            FileId = fileId;
            ExpirationDate = expirationDate;
            Status = status.ToString();
            StatusDisplay = statusDisplay;
        }

        public Guid? FileId { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Status { get; set; }
        public string StatusDisplay { get; set; }
    }
}
