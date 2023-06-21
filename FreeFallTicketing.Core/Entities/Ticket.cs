using SkyDiveTicketing.Core.Entities.Base;

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
            Cancelled = false;
        }

        public string TicketNumber { get; set; }
        public bool Voidable { get; set; }
        public User? ReservedBy { get; set; }
        public Guid? ReservedById { get; set; }
        public bool ReservedByAdmin { get; set; }
        public bool Paid { get; private set; }
        public bool Locked { get; private set; }
        public User? LockedBy { get; private set; }
        public bool Cancelled { get; private set; }
        public DateTime? ReserveTime { get; set; }

        /// <summary>
        /// با توجه به اینکه قیمت بلیت های ممکن است تغییر کند.
        /// </summary>
        public double PaidAmount { get; set; }
        public AdminCartable? RelatedAdminCartableRequest { get; private set; }

        public void SetRequest(AdminCartable request) => RelatedAdminCartableRequest = request;
        public void SetAsPaid(double amount)
        {
            Paid = true;
            PaidAmount = amount;
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
        public void SetAsCancelled() => Cancelled = true;

    }
}