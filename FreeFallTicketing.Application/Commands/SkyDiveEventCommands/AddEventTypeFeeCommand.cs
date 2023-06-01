using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts;

namespace SkyDiveTicketing.Application.Commands.SkyDiveEventCommands
{
    public class AddEventTypeFeeCommand : ICommandBase
    {
        public Guid SkyEventTypeId { get; set; }
        public List<EventTypeItemCommand> Items { get; set; }
        public void Validate()
        {
            Items.ForEach(x => x.Validate());
            new AddEventTypeFeeCommandValidator().Validate(this).RaiseExceptionIfRequired();
        }
    }

    public class EventTypeItemCommand : ICommandBase
    {
        public Guid TypeId { get; set; }
        public double Amount { get; set; }
        public void Validate() => new EventTypeItemCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
