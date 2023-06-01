using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts;

namespace SkyDiveTicketing.Application.Commands.SkyDiveEventCommands
{
    public class AddSkyDiveEventFlightCommand : ICommandBase
    {
        public int FlightNumber { get; set; }
        public int VoidableNumber { get; set; }

        public List<TicketTypeCommand> TickTypes { get; set; }

        public void Validate()
        {
            TickTypes.ForEach(c=> c.Validate());
            new AddSkyDiveEventFlightCommandValidator().Validate(this).RaiseExceptionIfRequired();
        }
    }

    public class TicketTypeCommand : ICommandBase
    {
        public Guid TypeId { get; set; }
        public int Qty { get; set; }

        public void Validate() => new TicketTypeCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}