using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.SkyDiveEventValidatorts;

namespace SkyDiveTicketing.Application.Commands.SkyDiveEventCommands
{
    public class SkyDiveEventUpdateCommand : ICommandBase
    {
        public string? Title { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Voidable { get; set; }
        public Guid? Image { get; set; }
        public Guid StatusId { get; set; }
        public bool SubjecToVAT { get; set; }

        public void Validate() => new SkyDiveEventUpdateCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
