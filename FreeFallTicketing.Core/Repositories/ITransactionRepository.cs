using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task AddTransaction(string ticketNumber, string eventName, string paymetInformation, double amount, TransactionType type, User payer);
    }
}
