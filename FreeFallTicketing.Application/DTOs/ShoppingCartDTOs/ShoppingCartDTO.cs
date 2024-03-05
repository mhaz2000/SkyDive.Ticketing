using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDiveTicketing.Application.DTOs.ShoppingCartDTOs
{
    public class ShoppingCartDTO : BaseDTO<Guid>
    {
        public ShoppingCartDTO(Guid id, DateTime createdAt, DateTime updatedAt)
            : base(id, createdAt, updatedAt)
        {
        }

        public Guid SkyDiveEventId { get; set; }
        public int TicketsCount => Items.Count();
        public double TaxAmount { get; set; }
        public double TotalAmount { get; set; }
        public List<ShoppingCartItemDTO> Items { get; set; }
        public double PayableAmount => TotalAmount + TaxAmount;
    }

    public class ShoppingCartItemDTO
    {

        public ShoppingCartItemDTO(int flightNumber, DateTime? flightDate, int qty, string type, double amount, bool subjectToVAT, Guid flightLoadId, Guid ticketTypeId, IEnumerable<string> ticketsNumber)
        {
            FlightNumber = flightNumber;
            UserCode = qty;
            Type = type;
            Amount = amount;
            SubjectToVAT = subjectToVAT;
            FlightLoadId = flightLoadId;
            TicketTypeId = ticketTypeId;
            TicketsNumber = ticketsNumber;
            FlightDate = flightDate;
        }

        public bool SubjectToVAT { get; set; }
        public int FlightNumber { get; set; }
        public int UserCode { get; set; }
        public string Type { get; set; }
        public Guid TicketTypeId { get; set; }
        public IEnumerable<string> TicketsNumber { get; set; }
        public DateTime? FlightDate { get; set; }
        public double Amount { get; set; }
        public Guid FlightLoadId { get; set; }

    }
}
