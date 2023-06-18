using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.SkyDiveEventCommands;
using SkyDiveTicketing.Application.Services.SkyDiveEventServices;
using System.Data;

namespace SkyDiveTicketing.API.Controllers.SkyDiveEvents
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkyDiveEventStatusesController : ApiControllerBase
    {
        private readonly ISkyDiveEventStatusService _skyDiveEventStatusService;

        public SkyDiveEventStatusesController(ISkyDiveEventStatusService skyDiveEventStatusService)
        {
            _skyDiveEventStatusService = skyDiveEventStatusService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(SkyDiveEventStatusCommand command)
        {
            try
            {
                command.Validate();

                await _skyDiveEventStatusService.Create(command);
                return OkResult("وضعیت رویداد جدید با موفقیت ثبت شد.");
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

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SkyDiveEventStatusCommand command)
        {
            try
            {
                command.Validate();

                await _skyDiveEventStatusService.Update(command, id);
                return OkResult("وضعیت رویداد با موفقیت ویرایش شد.");
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

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var statuses = _skyDiveEventStatusService.GetStatuses();
                return OkResult("وضعیت های رویداد", statuses, statuses.Count());
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var flightLoad = await _skyDiveEventStatusService.GetStatus(id);
                return OkResult("وضعیت رویداد.", flightLoad);
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _skyDiveEventStatusService.Remove(id);
                return OkResult("وضعیت رویداد با موفقیت حذف شد.");
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
