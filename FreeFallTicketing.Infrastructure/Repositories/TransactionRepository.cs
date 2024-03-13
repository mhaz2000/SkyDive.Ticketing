using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class TransactionRepository : Repository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(DataContext context) : base(context)
        {
        }

        public int AddTransaction(string ticketNumber, string eventName, string paymetInformation, double amount, TransactionType type, User payer,
            bool subjectToVat, int? invoiceNumber = null)
        {
            var settings = Context.Settings.SingleOrDefault();

            int number = invoiceNumber ?? Context.Transactions.OrderByDescending(s => s.InvoiceNumber).FirstOrDefault()?.InvoiceNumber ?? 0;
            Context.Transactions.Add(new Transaction(ticketNumber, eventName, paymetInformation, amount, type, subjectToVat ? number : ++number, payer,
               subjectToVat ? Math.Truncate(amount * settings.VAT / 100) : 0));

            return number;
        }
    }
}
