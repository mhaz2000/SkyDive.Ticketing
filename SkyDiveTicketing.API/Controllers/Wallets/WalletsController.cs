using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.WalletCommands;
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
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> ChargeUserWallet(ChargeUserWalletCommand command)
        {
            try
            {
                command.Validate();

                await _walletService.ChargeUserWalletByAdmin(command);
                return OkResult("کیف پول کاربر با موفقیت شارژ شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
