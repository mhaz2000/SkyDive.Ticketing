using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class Transaction : BaseEntity
    {
        public Transaction()
        {

        }

        public Transaction(string ticketNumber, string eventName, string paymetInformation, double amount, TransactionType type, int? invoiceNumber, User payer, double vat) : base()
        {
            InvoiceNumber = invoiceNumber;
            EventName = eventName;
            TicketNumber = ticketNumber;
            PaymentInformation = paymetInformation;
            Amount = amount;
            Type = type;
            Payer = payer;
            VAT= vat;
        }

        /// <summary>
        /// اطلاعات بلیت ها
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
        /// مالیات بر ارزش افزوده
        /// </summary>
        public double VAT { get; set; }

        /// <summary>
        /// مبلغ کل
        /// </summary>
        public double TotalAmount => VAT + Amount;

        /// <summary>
        /// نوع
        /// </summary>
        public TransactionType Type { get; set; }

        /// <summary>
        /// شماره فاکتور
        /// </summary>
        public int? InvoiceNumber { get; set; }

        /// <summary>
        /// پرداخت کننده
        /// </summary>
        public User Payer { get; set; }
    }
    public enum TransactionType
    {
        Confirmed,
        Revoked
    }
}
