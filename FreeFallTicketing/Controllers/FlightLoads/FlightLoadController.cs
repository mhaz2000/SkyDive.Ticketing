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
    public class FlightLoadController : ApiControllerBase
    {
        private readonly IFlightLoadService _flightLoadService;
        public FlightLoadController(IFlightLoadService flightLoadService)
        {
            _flightLoadService = flightLoadService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(FlightLoadCommand command)
        {
            try
            {
                command.Validate();

                await _flightLoadService.Create(command);
                return OkResult("لود پرواز با موفقیت ثبت شد.");
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
        public async Task<IActionResult> Update(Guid id, [FromBody] FlightLoadCommand command)
        {
            try
            {
                command.Validate();

                await _flightLoadService.Update(command, id);
                return OkResult("لود پرواز با موفقیت ویرایش شد.");
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
        public IActionResult Get(string search, PageQuery page)
        {
            try
            {
                var flightLoads = _flightLoadService.GetAllFlighLoads(search);
                return OkResult("اطلاعات لود های پرواز.", flightLoads.ToPagingAndSorting(page), flightLoads.Count());
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
                var flightLoad = await _flightLoadService.GetFlighLoad(id);
                return OkResult("اطلاعات لود پرواز.", flightLoad);
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
                await _flightLoadService.Remove(id);
                return OkResult("اطلاعات لود پرواز با موفقیت حذف شد.");
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
