using SkyDiveTicketing.Application.Commands.Base;
using SkyDiveTicketing.Application.Validators.Extensions;
using SkyDiveTicketing.Application.Validators.FlightLoadValidators;
using SkyDiveTicketing.Application.Validators.ReservationValidators;

namespace SkyDiveTicketing.Application.Commands.Reservation
{
    public class RegisterIdentityDocumentCommand : ICommandBase
    {
        public Guid TicketId { get; set; }

        public ICollection<RegisterPassengerIdentityDocumentCommand> Documents { get; set; }

        public void Validate() => new RegisterIdentityDocumentCommandValidator().Validate(this).RaiseExceptionIfRequired();

    }

    public class RegisterPassengerIdentityDocumentCommand : ICommandBase
    {
        public string NationalCode { get; set; }
        public Guid CityId { get; set; }
        public string Address { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyPhone { get; set; }
        public Guid MedicalDocumentFileId { get; set; }
        public Guid LogBookDocumentFileId { get; set; }
        public Guid AttorneyDocumentFileId { get; set; }
        public Guid NationalCardDocumentFileId { get; set; }

        public void Validate() => new RegisterPassengerIdentityDocumentCommandValidator().Validate(this).RaiseExceptionIfRequired();
    }
}
