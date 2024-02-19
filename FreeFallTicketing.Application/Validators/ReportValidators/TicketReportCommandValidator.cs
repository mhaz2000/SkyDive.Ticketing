using FluentValidation;
using SkyDiveTicketing.Application.Commands.ReportCommands;

namespace SkyDiveTicketing.Application.Validators.ReportValidators
{
    internal class TicketReportCommandValidator : AbstractValidator<TicketReportCommand>
    {
        public TicketReportCommandValidator()
        {
        }
    }
}
