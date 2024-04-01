using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyDiveTicketing.API.Base;
using SkyDiveTicketing.Application.Commands.ReportCommands;
using SkyDiveTicketing.Application.DTOs.ReportDTOs;
using SkyDiveTicketing.Application.Services.ReportServices;

namespace SkyDiveTicketing.API.Controllers.Reports
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ApiControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("TicketsReport")]
        public async Task<IActionResult> GetTicketsReport([FromQuery] PageQuery pageQuery, [FromBody] TicketReportCommand command)
        {
            try
            {
                command.Validate();

                (var data, var total, var cacheId) = await _reportService.GetTicketsReport(command);
                return OkResult(cacheId.ToString(), data.AsEnumerable().ToPagingAndSorting(pageQuery), total);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                return BadRequest("متاسفانه خطای سیستمی رخ داده");
            }
        }

        [HttpPut("PrintTicketsReport/{id}")]
        public async Task<IActionResult> PrintTicketsReport(Guid id)
        {
            try
            {
                var stream = await _reportService.PrintTicketsReport(id);
                return File(stream, "application/octet-stream");
            }
            catch (Exception)
            {
                return BadRequest("متاسفانه خطای سیستمی رخ داده");
            }

        }
    }
}
