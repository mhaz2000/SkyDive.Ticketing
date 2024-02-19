using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.ReportValidators;

namespace SkyDiveTicketing.Application.Commands.ReportCommands
{
    public class TicketReportCommand : ICommandBase
    {
        public string Search { get; set; } = string.Empty;
        public IEnumerable<Guid> EventsId { get; set; } = Enumerable.Empty<Guid>();

        public void Validate() => new TicketReportCommandValidator().Validate(this).RaiseExceptionIfRequired();

    }
}
