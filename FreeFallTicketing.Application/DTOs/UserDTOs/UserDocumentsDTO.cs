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
            MedicalDocuments = new List<UserDocumentDetailDTO>();
            LogBookDocuments = new List<UserDocumentDetailDTO>();
            AttorneyDocuments = new List<UserDocumentDetailDTO>();
            NationalCardDocuments = new List<UserDocumentDetailDTO>();

        }

        /// <summary>
        /// فایل سند پزشکی
        /// </summary>
        public IEnumerable<UserDocumentDetailDTO> MedicalDocuments { get; set; }

        /// <summary>
        /// سند لاگ بوک
        /// </summary>
        public IEnumerable<UserDocumentDetailDTO> LogBookDocuments { get; set; }

        /// <summary>
        /// سند وکالت نامه محضری
        /// </summary>
        public IEnumerable<UserDocumentDetailDTO> AttorneyDocuments { get; set; }

        /// <summary>
        /// فایل کارت ملی
        /// </summary>
        public IEnumerable<UserDocumentDetailDTO> NationalCardDocuments { get; set; }
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
