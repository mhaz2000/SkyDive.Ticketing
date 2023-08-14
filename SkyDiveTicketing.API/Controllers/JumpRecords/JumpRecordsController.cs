using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.JumpRecordCommands;
using SkyDiveTicketing.Application.Services.JumpRecordServices;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SkyDiveTicketing.API.Controllers.JumpRecords
{
    [Route("api/[controller]")]
    [ApiController]
    public class JumpRecordsController : ApiControllerBase
    {
        private readonly IJumpRecordService _jumpRecordService;

        public JumpRecordsController(IJumpRecordService jumpRecordService)
        {
            _jumpRecordService = jumpRecordService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(JumpRecordCommand command)
        {
            try
            {
                command.Validate();

                await _jumpRecordService.Create(command, command.UserId ?? UserId, UserId);
                return OkResult("سابقه پرش با موفقیت اضافه شد.");
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(Guid id)
        {
            try
            {
                await _jumpRecordService.Remove(id);
                return OkResult("سابقه پرش با موفقیت حذف شد.");
            }
            catch (ManagedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetJumpRecords([FromQuery] PageQuery pageQuery, Guid? userId)
        {
            try
            {
                var jumpRecords = await _jumpRecordService.GetJumpRecords(userId ?? UserId);
                return OkResult("سوابق پرش", jumpRecords.ToPagingAndSorting(pageQuery), jumpRecords.Count());
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

        [HttpGet("AllJumpRecords")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetJumpRecords([FromQuery] PageQuery pageQuery)
        {
            try
            {
                var jumpRecords = await _jumpRecordService.GetAllJumpRecords();
                return OkResult("سوابق پرش", jumpRecords.ToPagingAndSorting(pageQuery), jumpRecords.Count());
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

        [HttpPut("ConfirmJumpRecord/{id}/{isConfirmed}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfirmJumpRecord(Guid id, bool isConfirmed)
        {
            try
            {
                await _jumpRecordService.ConfirmJumpRecord(id, isConfirmed);
                return OkResult(isConfirmed ? "سابقه پرش با موفقیت تایید شد." : "سابقه پرش رد شد.");
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
