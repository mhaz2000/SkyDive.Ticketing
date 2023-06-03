using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class Ticket : BaseEntity
    {
        public Ticket(string ticketNumber, bool voidable, User reservedBy, bool reservedByAdmin) : base()
        {
            TicketNumber = ticketNumber;
            Voidable = voidable;
            ReservedBy = reservedBy;
            ReservedByAdmin = reservedByAdmin;
            Paid = false;
        }

        public string TicketNumber { get; set; }
        public bool Voidable { get; set; }
        public User ReservedBy { get; set; }
        public bool ReservedByAdmin { get; set; }
        public bool Paid { get; private set; }

        public void SetAsPaid() => Paid = true;
    }
}