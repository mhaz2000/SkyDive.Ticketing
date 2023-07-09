using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Services.TransactionServices;
using System.Data;

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
        public async Task<IActionResult> Get([FromQuery] PageQuery pageQuery)
        {
            try
            {
                var transactions = await _transactionService.GetTransactions(UserId);
                return OkResult("اطلاعات تراکنش ها", transactions.ToPagingAndSorting(pageQuery), transactions.Count());
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetUserTransactions/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserTransactions(Guid userId, [FromQuery] PageQuery pageQuery)
        {
            try
            {
                var transactions = await _transactionService.GetTransactions(userId);
                return OkResult("اطلاعات تراکنش ها", transactions.ToPagingAndSorting(pageQuery), transactions.Count());
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
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
        }
    }
}
