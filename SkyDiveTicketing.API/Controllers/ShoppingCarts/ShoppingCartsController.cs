using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.PaymentServices;
using SkyDiveTicketing.Application.Services.ReservationServices;
using SkyDiveTicketing.Application.Services.ShoppingCartCheckoutServices;

namespace SkyDiveTicketing.API.Controllers.ShoppingCarts
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ApiControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IShoppingCartCheckoutService _shoppingCartCheckoutService;

        public ShoppingCartsController(IReservationService reservationService, IShoppingCartCheckoutService shoppingCartCheckoutService)
        {
            _reservationService = reservationService;
            _shoppingCartCheckoutService = shoppingCartCheckoutService;
        }

        [HttpGet("CheckTickets")]
        public async Task<IActionResult> CheckTickets()
        {
            try
            {
                (bool res, string message) = await _reservationService.CheckTickets(UserId);

                return res ? OkResult("مشکلی در سبد خرید وجود ندارد.") : BadRequest(message);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("Checkout")]
        public async Task<IActionResult> ShoppingCartCheckout()
        {
            try
            {
                var redirectUrl = await _shoppingCartCheckoutService.Checkout(UserId);
                return OkResult(redirectUrl);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Verify")]
        public async Task<IActionResult> PaymentVerfication(string authority)
        {
            try
            {
                var data = await _shoppingCartCheckoutService.Verfiy(UserId, authority);
                return OkResult("پرداخت با موفقیت انجام شد.", data);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserShoppingCart()
        {
            try
            {
                var shoppingCart = await _reservationService.GetUserShoppingCart(UserId);

               return OkResult("سبد خرید کاربر", shoppingCart);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> RemoveShoppingCart()
        {
            try
            {
                await _reservationService.RemoveShoppingCart(UserId);

                return OkResult("سبد خرید با موفقیت حذف شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
