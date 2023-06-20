using MD.PersianDateTime;
using System.Globalization;

namespace SkyDiveTicketing.Application.DTOs.TicketDTOs
{
    public class ReservingTicketDTO
    {
        PersianCalendar pc = new PersianCalendar();
        public ReservingTicketDTO(DateTime date, IEnumerable<TicketFlightDTO> ticketFlights)
        {
            Date = date;
            Flights = ticketFlights;
        }

        public DateTime Date { get; set; }
        public string DateDisplay => $"{pc.GetDayOfMonth(Date)} {PersianDateTime.GetPersianMonthName(pc.GetMonth(Date))}";
        public IEnumerable<TicketFlightDTO> Flights { get; set; }
        public int Qty { get; set; }

    }

    public class TicketFlightDTO
    {
        public TicketFlightDTO(int flightNumber, IEnumerable<TicketDetailDTO> tickets, Guid flightId)
        {
            FlightNumber = flightNumber;
            Tickets = tickets;
            FlightId = flightId;
        }

        public int FlightNumber { get; set; }

        public Guid FlightId { get; set; }

        public IEnumerable<TicketDetailDTO> Tickets { get; set; }
    }
    
    public class TicketDetailDTO
    {
        public TicketDetailDTO(string ticketType, double amount, int qty)
        {
            TicketType = ticketType;
            Amount = amount;
            Qty = qty;
        }

        public string TicketType { get; set; }
        public double Amount { get; set; }
        public int Qty { get; set; }
    }
}
