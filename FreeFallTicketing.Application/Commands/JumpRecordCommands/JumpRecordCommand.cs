using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.JumpRecordValidators;

namespace SkyDiveTicketing.Application.Commands.JumpRecordCommands
{
    public class JumpRecordCommand : ICommandBase
    {
        public DateTime Date { get; set; }
        public string? Location { get; set; }
        public string? Equipments { get; set; }
        public string? PlaneType { get; set; }
        public float Height { get; set; }
        public TimeOnly Time { get; set; }
        public string? Description { get; set; }

        public void Validate() => new JumpRecordCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
