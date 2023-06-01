
namespace SkyDiveTicketing.Application.DTOs.SkyDiveEventDTOs
{
    public class SkyDiveEventDTO : BaseDTO<Guid>
    {
        public SkyDiveEventDTO(Guid id, DateTime createdAt, DateTime updatedAt, string title, DateTime startDate,
            DateTime endDate, Guid image, bool isActive, int capacity, string code, string location, bool subjectToVAT, bool voidable, string termsAndConditions, string statusTitle)
            : base(id, createdAt, updatedAt)
        {
            Title = title;
            StartDate = startDate;
            EndDate = endDate;
            Image = image;
            IsActive = isActive;
            Capacity = capacity;
            Code = code;
            Location = location;
            SubjecToVAT = subjectToVAT;
            TermsAndConditions = termsAndConditions;
            StatusTitle = statusTitle;
            Voidable = voidable;
        }

        public string Code { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public Guid Image { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Capacity { get; set; }
        public bool SubjecToVAT { get; set; }
        public bool IsActive { get; set; }
        public bool Voidable { get; set; }
        public string? TermsAndConditions { get; set; }
        public string StatusTitle { get; set; }
    }
}
