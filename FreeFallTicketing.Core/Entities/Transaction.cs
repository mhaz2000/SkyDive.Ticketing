using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class Transaction : BaseEntity
    {
        public Transaction(string ticketNumber, string eventName, string paymetInformation, double amount, TransactionType type, int invoiceNumber) : base()
        {
            InvoiceNumber = invoiceNumber;
            EventName = eventName;
            TicketNumber = ticketNumber;
            PaymentInformation = paymetInformation;
            Amount = amount;
            Type = type;
        }

        /// <summary>
        /// شماره بلیت
        /// </summary>
        public string TicketNumber { get; set; }

        /// <summary>
        /// نام رویداد
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// اطلاعات پرداخت
        /// </summary>
        public string PaymentInformation { get; set; }

        /// <summary>
        /// مبلغ
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// نوع
        /// </summary>
        public TransactionType Type { get; set; }

        /// <summary>
        /// شماره فاکتور
        /// </summary>
        public int InvoiceNumber { get; set; }
    }
    public enum TransactionType
    {
        Confirmed,
        Revoked
    }
}
