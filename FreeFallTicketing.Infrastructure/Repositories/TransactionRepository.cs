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

        public async Task<int> AddTransaction(string ticketNumber, string eventName, string paymetInformation, double amount, TransactionType type, User payer,
            bool walletCharging, int? invoiceNumber = null)
        {
            var settings = Context.Settings.SingleOrDefault();

            int number = invoiceNumber ?? Context.Transactions.OrderByDescending(s => s.InvoiceNumber).FirstOrDefault()?.InvoiceNumber ?? 0;
            await Context.Transactions.AddAsync(new Transaction(ticketNumber, eventName, paymetInformation, amount, type, walletCharging ? number : ++number, payer,
                Math.Truncate(amount * settings.VAT/100)));

            return number;
        }
    }
}
