using SkyDiveTicketing.Application.Commands.ReportCommands;
using SkyDiveTicketing.Application.DTOs.ReportDTOs;

namespace SkyDiveTicketing.Application.Services.ReportServices
{
    public interface IReportService
    {
        Task<(IQueryable<TicketReportDTO> Data, int Total)> GetTicketsReport(TicketReportCommand command);
    }
}
