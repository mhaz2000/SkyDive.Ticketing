using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Services.ReservationServices;

namespace SkyDiveTicketing.API.Controllers.ShoppingCarts
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ApiControllerBase
    {
        private readonly IReservationService _reservationService;

        public ShoppingCartsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
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
            catch (Exception ex)
            {
                Console.WriteLine(ex+"\n----------------------");
                return BadRequest("متاسفانه خطای سیستمی رخ داده");
            }
        }
    }
}
