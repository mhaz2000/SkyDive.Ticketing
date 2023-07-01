using SkyDiveTicketing.Core.Entities.Base;
using System.Net.Sockets;

namespace SkyDiveTicketing.Core.Entities
{
    public class Ticket : BaseEntity
    {
        public Ticket()
        {

        }
        public Ticket(string ticketNumber, bool voidable, User? reservedBy, bool reservedByAdmin) : base()
        {
            TicketNumber = ticketNumber;
            Voidable = voidable;
            ReservedBy = reservedBy;
            ReservedByAdmin = reservedByAdmin;
            Paid = false;
            Locked = false;
            ConfirmedByAdmin = false;
            CancellationRequest = false;
        }

        public string TicketNumber { get; set; }
        public bool Voidable { get; set; }
        public User? ReservedBy { get; set; }
        public Guid? ReservedById { get; set; }
        public bool ReservedByAdmin { get; set; }
        public bool Paid { get; private set; }
        public bool Locked { get; private set; }
        public User? LockedBy { get; private set; }
        public DateTime? ReserveTime { get; set; }
        public DateTime? PaidTime { get; set; }
        public User? PaidBy { get; set; }
        public bool ConfirmedByAdmin { get; set; }
        public bool CancellationRequest { get; set; }

        public Guid? SkyDiveEventId { get; set; }
        public int? FlightNumber { get; set; }
        public string? TicketType { get; set; }
        public DateTime? FlightDate { get; set; }


        /// <summary>
        /// با توجه به اینکه قیمت بلیت های ممکن است تغییر کند.
        /// </summary>
        public double PaidAmount { get; set; }
        public AdminCartable? RelatedAdminCartableRequest { get; set; }

        public void SetRequest(AdminCartable request)
        {
            RelatedAdminCartableRequest = request;
            CancellationRequest= true;
        }
        public void SetAsPaid(double amount, Guid skyDiveEventId, int flightNumber, string ticketType, DateTime flightDate)
        {
            Paid = true;
            PaidAmount = amount;
            SkyDiveEventId = skyDiveEventId;
            FlightNumber = flightNumber;
            TicketType = ticketType;
            FlightDate = flightDate;
        }

        public void SetAsUnPaid()
        {
            Paid = false;
            PaidAmount = 0;
            SkyDiveEventId = null;
            FlightNumber = null;
            TicketType = null;
            FlightDate = null;
            PaidTime = null;
            ReservedBy = null;
            PaidBy = null;
        }
        public void SetAsUnLock()
        {
            Locked = false;
            LockedBy = null;
        }
        public void SetAsLock(User user)
        {
            LockedBy = user;
            Locked = true;
        }
    }

    public class CancelledTicket : BaseEntity
    {
        public CancelledTicket() : base() { }

        public CancelledTicket(string ticketNumber, User? reservedBy, DateTime? reserveTime, DateTime? paidTime, User? paidBy, Guid? skyDiveEventId,
            int? flightNumber, string? ticketType, DateTime? flightDate, double paidAmount)
        {
            TicketNumber = ticketNumber;
            ReservedBy = reservedBy;
            ReserveTime = reserveTime;
            PaidTime = paidTime;
            PaidBy = paidBy;
            SkyDiveEventId = skyDiveEventId;
            FlightNumber = flightNumber;
            TicketType = ticketType;
            FlightDate = flightDate;
            PaidAmount = paidAmount;
        }

        public string TicketNumber { get; set; }
        public User? ReservedBy { get; set; }
        public DateTime? ReserveTime { get; set; }
        public DateTime? PaidTime { get; set; }
        public User? PaidBy { get; set; }
        public Guid? SkyDiveEventId { get; set; }
        public int? FlightNumber { get; set; }
        public string? TicketType { get; set; }
        public DateTime? FlightDate { get; set; }
        public double? PaidAmount { get; set; }
    }
}