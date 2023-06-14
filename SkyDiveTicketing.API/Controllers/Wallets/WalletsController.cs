using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Services.WalletServices;

namespace SkyDiveTicketing.API.Controllers.Wallets
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ApiControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletsController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserWallet()
        {
            try
            {
                var wallet = await _walletService.GetUserWallet(UserId);
                return OkResult("کیف پول کاربر", wallet);
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
