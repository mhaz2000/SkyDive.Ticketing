
using MD.PersianDateTime;
using System.Globalization;

namespace SkyDiveTicketing.Application.DTOs.SkyDiveEventDTOs
{
    public class SkyDiveEventDTO : BaseDTO<Guid>
    {
        PersianCalendar pc = new PersianCalendar();

        public SkyDiveEventDTO(Guid id, DateTime createdAt, DateTime updatedAt, string title, DateTime startDate,
            DateTime endDate, Guid image, bool isActive, int capacity, string code, string location, bool subjectToVAT, bool voidable,
            string termsAndConditions, string statusTitle, bool reservable, IEnumerable<SkyDiveEventDayDTO> days)
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
            Reservable = reservable;
            Days = days;
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
        public bool Reservable { get; set; }

        public string Duration =>
            $"{pc.GetDayOfMonth(StartDate)} {PersianDateTime.GetPersianMonthName(pc.GetMonth(StartDate))} تا {pc.GetDayOfMonth(EndDate)} {PersianDateTime.GetPersianMonthName(pc.GetMonth(EndDate))}";

        public IEnumerable<SkyDiveEventDayDTO> Days { get; set; }
    }

    public class SkyDiveEventDayDTO
    {
        public SkyDiveEventDayDTO(string date, Guid id)
        {
            Date = date;
            Id = id;
        }

        public string Date { get; set; }
        public Guid Id { get; set; }
    }
}
