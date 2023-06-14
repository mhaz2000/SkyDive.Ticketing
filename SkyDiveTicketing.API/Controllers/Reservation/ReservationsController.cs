using FluentValidation;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.Reservation;
using SkyDiveTicketing.Application.Services.ReservationServices;
using Microsoft.AspNetCore.Mvc;

namespace SkyDiveTicketing.API.Controllers.Reservation
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ApiControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ReserveCommand command)
        {
            try
            {
                command.Validate();

                await _reservationService.Update(command, UserId);
                return OkResult("بلیت با موفقیت ویرایش شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex+"\n----------------------");
                return BadRequest("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpGet("MyTickets")]
        public async Task<IActionResult> GetMyTickets(PageQuery pageQuery)
        {
            try
            {
                var tickets = await _reservationService.GetUserTickets(UserId);
                return OkResult("بلیت های من.", tickets.ToPagingAndSorting(pageQuery), tickets.Count());
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

        [HttpPut("SetAsPaid")]
        public async Task<IActionResult> SetAsPaid()
        {
            try
            {
                var result = await _reservationService.SetAsPaid(UserId);
                return OkResult("پرداخت موفقیت آمیز بود.");
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

        [HttpPut("CancellingTicketRequest/{id}")]
        public async Task<IActionResult> CancelTicketRequest(Guid id)
        {
            try
            {
                await _reservationService.CancelTicketRequest(id, UserId);
                return OkResult("درخواست لغو بلیت با موفقیت ارسال شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex+"\n----------------------");
                return BadRequest("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpPut("CancellingTicketResponse/{id}/{response}")]
        public async Task<IActionResult> CancellingTicketResponse(Guid id, bool response)
        {
            try
            {
                await _reservationService.CancelTicketResponse(id, response);
                return OkResult($"درخواست کنسلی بلیت {(response ? "تایید" : "رد")} شد");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex+"\n----------------------");
                return BadRequest("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpGet("PrintTicket/{id}")]
        public IActionResult PrintTicket(Guid id)
        {
            try
            {
                var ticketFile = _reservationService.PrintTicket(id);
                return File(ticketFile, "application/octet-stream");
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
