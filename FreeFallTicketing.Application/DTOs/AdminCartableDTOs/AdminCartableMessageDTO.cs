using SkyDiveTicketing.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDiveTicketing.Application.DTOs.AdminCartableDTOs
{
    public class AdminCartableMessageDTO : BaseDTO<Guid>
    {
        public AdminCartableMessageDTO(Guid id, DateTime createdAt, DateTime updatedAt, string title, Guid applicantId, string applicationName, bool done,
            RequestType requestType, string requestTypeDisplay, int applicantCode, string applicantTypeDisplay, Guid applicantTypeId) : base(id, createdAt, updatedAt)
        {
            RequestTypeDisplay = requestTypeDisplay;
            Title = title;
            ApplicantId = applicantId;
            ApplicantName = applicationName;
            Done = done;
            RequestType = requestType;
            ApplicantCode = applicantCode;
            ApplicantTypeDisplay = applicantTypeDisplay;
            ApplicantTypeId = applicantTypeId;
        }

        public string Title { get; set; }
        public Guid ApplicantId { get; set; }
        public string ApplicantName { get; set; }
        public int ApplicantCode { get; set; }
        public Guid ApplicantTypeId { get; set; }
        public string ApplicantTypeDisplay { get; set; }
        public bool Done { get; private set; }
        public RequestType RequestType { get; set; }
        public string RequestTypeDisplay { get; set; }
        public TimeSpan Time => CreatedAt.ToLocalTime().TimeOfDay;
    }
}
