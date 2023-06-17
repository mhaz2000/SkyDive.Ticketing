using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.SettingsCommands;
using SkyDiveTicketing.Application.Services.SettingsServices;
using System.Data;

namespace SkyDiveTicketing.API.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ApiControllerBase
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] SettingsCommand command)
        {
            try
            {
                command.Validate();

                await _settingsService.Update(command);
                return OkResult("تنظیمات سامانه با موفقیت ویرایش شد.");
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
                Console.WriteLine(ex + "\n----------------------");
                return BadRequest("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var settings = await _settingsService.GetSettings();
                return OkResult("تنظیمات سامانه", settings);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "\n----------------------");
                return BadRequest("متاسفانه خطای سیستمی رخ داده");
            }
        }
    }
}
