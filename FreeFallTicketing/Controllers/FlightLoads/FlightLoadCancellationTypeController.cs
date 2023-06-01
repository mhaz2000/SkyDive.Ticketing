using FluentValidation;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.FlightLoadCommands;
using SkyDiveTicketing.Application.Services.FlightLoadServices;
using Microsoft.AspNetCore.Mvc;

namespace SkyDiveTicketing.API.Controllers.FlightLoads
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightLoadCancellationTypeController : ApiControllerBase
    {
        private readonly IFlightLoadCancellationTypeService _flightLoadCancellationRateService;
        public FlightLoadCancellationTypeController(IFlightLoadCancellationTypeService flightLoadCancellationRateService)
        {
            _flightLoadCancellationRateService = flightLoadCancellationRateService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(FlightLoadCancellationTypeCommand command)
        {
            try
            {
                command.Validate();

                await _flightLoadCancellationRateService.Create(command);
                return OkResult("نرخ‌های کنسلی لود پرواز با موفقیت ثبت شد.");
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
        public async Task<IActionResult> Update(Guid id, [FromBody] FlightLoadCancellationTypeDetailCommand command)
        {
            try
            {
                command.Validate();

                await _flightLoadCancellationRateService.Update(command, id);
                return OkResult("نرخ‌های کنسلی لود پرواز با ویرایش شد.");
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

        [HttpGet("FlighLoadCancellationTypes/{flightLoadId}")]
        public IActionResult Get(Guid flightLoadId, string search, PageQuery page)
        {
            try
            {
                var flightLoads = _flightLoadCancellationRateService.GetFlightLoadCancellationRates(search, flightLoadId);
                return OkResult("اطلاعات نرخ‌های کنسلی لود های پرواز.", flightLoads);
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
        public IActionResult GetDetail(Guid id)
        {
            try
            {
                var flightLoad = _flightLoadCancellationRateService.GetFlightLoadCancellationType(id);
                return OkResult("اطلاعات نرخ کنسلی لود پرواز.", flightLoad);
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _flightLoadCancellationRateService.Remove(id);
                return OkResult("نزخ کنسلی لود پرواز با موفقیت حذف شد.");
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
