using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Services.TransactionServices;

namespace SkyDiveTicketing.API.Controllers.Transactions
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ApiControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(PageQuery pageQuery)
        {
            try
            {
                var transactions = await _transactionService.GetTransactions(UserId);
                return OkResult("اطلاعات تراکنش ها", transactions, transactions.Count());
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpGet("Print/{id}")]
        public async Task<IActionResult> Print(Guid id)
        {
            try
            {
                var invoiceFile = await _transactionService.PrintInvoice(id);
                return File(invoiceFile, "application/octet-stream");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest("متاسفانه خطای سیستمی رخ داده");
            }
        }
    }
}
