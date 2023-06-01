using FluentValidation;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.FlightLoadCommands;
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

        [HttpPost]
        public async Task<IActionResult> Create(ReserveCommand command)
        {
            try
            {
                command.Validate();

                await _reservationService.Create(command);
                return OkResult("رزرو اولیه با موفقیت انجام شد.");
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
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ReserveCommand command)
        {
            try
            {
                command.Validate();

                await _reservationService.Update(command, id);
                return OkResult("بلیط با موفقیت ویرایش شد.");
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
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpPut("RegisterIdentityDocuments")]
        public async Task<IActionResult> RegisterIdentityDocuments([FromBody] RegisterIdentityDocumentCommand command)
        {
            try
            {
                command.Validate();

                await _reservationService.RegisterIdentityDocuments(command);
                return OkResult("بلیط با موفقیت ویرایش شد.");
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
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpPut("CancelTicket/{id}")]
        public async Task<IActionResult> CancelTicket(Guid id)
        {
            try
            {
                await _reservationService.CancelTicket(id);
                return OkResult("بلیط با موفقیت کنسل شد.");
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
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            try
            {
                var tickets = await _reservationService.GetTickets();
                return OkResult("اطلاعات بلیط های خریداری شده.", tickets);
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
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
            }
        }
    }
}
