using SkyDiveTicketing.Application.Helpers;
using System.ComponentModel;

namespace SkyDiveTicketing.Application.DTOs.TicketDTOs
{
    public class MyTicketDTO : BaseDTO<Guid>
    {
        public MyTicketDTO(Guid id, DateTime createdAt, DateTime updatedAt, string ticketNumber, DateTime date, string flightNumber,
            string eventLocation, string ticketType, string termsAndConditionUrl, bool voidable, Guid skyDiveEventId, int skyDiveEventNumber, TicketStatus ticketStatus)
            : base(id, createdAt, updatedAt)
        {
            TicketNumber = ticketNumber;
            Date = date;
            FlightNumber = flightNumber;
            EventLocation = eventLocation;
            TicketType = ticketType;
            TermsAndConditionUrl = termsAndConditionUrl;
            Voidable = voidable;
            SkyDiveEventId = skyDiveEventId;
            SkyDiveEventNumber= skyDiveEventNumber;
            TicketStatus = ticketStatus;
        }

        public string TicketNumber { get; set; }
        public DateTime Date { get; set; }
        public string FlightNumber { get; set; }
        public string EventLocation { get; set; }
        public string TicketType { get; set; }
        public string TermsAndConditionUrl { get; set; }
        public bool Voidable { get; set; }
        public Guid SkyDiveEventId { get; set; }
        public int SkyDiveEventNumber { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public string TicketStatusDisplay => TicketStatus.GetDescription();
    }

    public enum TicketStatus
    {
        [Description("برگزار شده")]
        Held,
        [Description("لغو شده")]
        Cancelled,
        [Description("رزرو شده")]
        Reserved
    }
}
