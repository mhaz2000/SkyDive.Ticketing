using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.DTOs.TransactionDTOs;
using SkyDiveTicketing.Application.Helpers;
using SkyDiveTicketing.Core.Repositories.Base;
using System.Globalization;

namespace SkyDiveTicketing.Application.Services.TransactionServices
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TransactionDTO>> GetTransactions(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربر یافت نشد.");

            var transactions = await _unitOfWork.TransactionRepositroy.GetListWithIncludeAsync("Payer", c => c.Payer == user);

            return transactions.Select(transaction => new TransactionDTO(transaction.Id, transaction.CreatedAt, transaction.UpdatedAt,
                transaction.CreatedAt, transaction.TicketNumber, transaction.EventName, transaction.PaymentInformation, transaction.TotalAmount, transaction.Type, transaction.InvoiceNumber));
        }

        public async Task<MemoryStream> PrintInvoice(Guid id)
        {
            PersianCalendar pc = new PersianCalendar();

            var transaction = await _unitOfWork.TransactionRepositroy.GetFirstWithIncludeAsync(c => c.Id == id, c => c.Payer);
            if (transaction is null)
                throw new ManagedException("تراکنش مورد نظر یافت نشد.");

            if (transaction.InvoiceNumber is null)
                throw new ManagedException("امکان چاپ تراکنش های ایجاد شده از طریق شارژ کیف پول، وجود ندارد.");

            return PdfHelper.InvoicePdf($"{pc.GetYear(transaction.CreatedAt)}/{pc.GetMonth(transaction.CreatedAt).ToString("00")}/{pc.GetDayOfMonth(transaction.CreatedAt).ToString("00")}",
                transaction.InvoiceNumber!.Value, transaction.Payer.FullName, transaction.Payer.NationalCode ?? "", transaction.Payer.PhoneNumber, transaction.Amount,
                transaction.TicketNumber, transaction.VAT, transaction.TotalAmount);
        }
    }
}
