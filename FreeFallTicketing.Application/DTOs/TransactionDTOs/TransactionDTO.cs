using SkyDiveTicketing.Core.Entities;

namespace SkyDiveTicketing.Application.DTOs.TransactionDTOs
{
    public class TransactionDTO : BaseDTO<Guid>
    {
        public TransactionDTO(Guid id, DateTime createdAt, DateTime updatedAt, DateTime date, string ticketNumber, string eventName,
            string paymentInformation, double amount, TransactionType type, int? invoiceNumber) : base(id, createdAt, updatedAt)
        {
            InvoiceNumber = invoiceNumber;
            Date= date;
            TicketNumber = ticketNumber;
            EventName = eventName;
            PaymentInformation = paymentInformation;
            Amount = amount;
            Type = type;
        }

        public DateTime Date { get; set; }
        public string TicketNumber { get; set; }
        public string EventName { get; set; }
        public string PaymentInformation { get; set; }
        public double Amount { get; set; }
        public TransactionType Type { get; set; }
        public int? InvoiceNumber { get; set; }
    }
}
