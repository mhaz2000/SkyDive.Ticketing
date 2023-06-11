using SkyDiveTicketing.Application.DTOs.TransactionDTOs;

namespace SkyDiveTicketing.Application.Services.TransactionServices
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDTO>> GetTransactions(Guid userId);
        Task<MemoryStream> PrintInvoice(Guid id);
    }
}
