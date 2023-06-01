using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;
using SkyDiveTicketing.Application.Services.FlightLoadServices;

namespace SkyDiveTicketing.API.Controllers.SkyDiveEvents
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SkyDiveEventTicketTypeController : ApiControllerBase
    {
        private readonly ISkyDiveEventTicketTypeService _skyDiveEventTicketTypeService;

        public SkyDiveEventTicketTypeController(ISkyDiveEventTicketTypeService skyDiveEventTicketTypeService)
        {
            _skyDiveEventTicketTypeService = skyDiveEventTicketTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var skyDiveEventTicketTypes = await _skyDiveEventTicketTypeService.GetAllSkyDiveEventTicketTypes();
                return OkResult("انواع بلیط.", skyDiveEventTicketTypes, skyDiveEventTicketTypes.Count());
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(SkyDiveEventTicketTypeCommand command)
        {
            try
            {
                command.Validate();

                await _skyDiveEventTicketTypeService.Create(command);
                return OkResult("نوع بلیط جدید با موفقیت ثبت شد.");
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
        public async Task<IActionResult> Update(Guid id, [FromBody] SkyDiveEventTicketTypeCommand command)
        {
            try
            {
                command.Validate();

                await _skyDiveEventTicketTypeService.Update(command, id);
                return OkResult("نوع بلیط با موفقیت ویرایش شد.");
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var skyDiveEventType = await _skyDiveEventTicketTypeService.GetSkyDiveEventTicketType(id);
                return OkResult("وضعیت رویداد.", skyDiveEventType);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new SystemException("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _skyDiveEventTicketTypeService.Remove(id);
                return OkResult("نوع بلیط با موفقیت حذف شد.");
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
