using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Services.CityServices;

namespace SkyDiveTicketing.API.Controllers.Cities
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ApiControllerBase
    {
        private readonly ICityService _cityService;

        public CitiesController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] PageQuery pageQuery, string? search = "")
        {

            try
            {
                var cities = _cityService.GetCities(search);
                return OkResult("اطلاعات شخصی شما با موفقیت به ثبت رسید.", cities.ToPagingAndSorting(pageQuery), cities.Count());
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
