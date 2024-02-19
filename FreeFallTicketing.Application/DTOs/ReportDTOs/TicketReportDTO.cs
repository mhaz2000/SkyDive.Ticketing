namespace SkyDiveTicketing.Application.DTOs.ReportDTOs
{
    public record TicketReportDTO
    {
        public int EventCode { get; set; }
        public string EventTitle { get; set; }
        public string EventDate { get; set; }
        public string FlightName { get; set; }
        public string FlightStatus { get; set; }
        public string FlightDate { get; set; }
        public int FlightNumber { get; set; }
        public string TicketNumber { get; set; }
        public string TicketType { get; set; }
        public string TicketStatus { get; set; }
        public string FullName { get; set; }
        public string NationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public float? Weight { get; set; }
        public float? Height { get; set; }
        public int UserCode { get; internal set; }
    }
}
