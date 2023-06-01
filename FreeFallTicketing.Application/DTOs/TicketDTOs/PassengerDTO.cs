namespace SkyDiveTicketing.Application.DTOs.TicketDTOs
{
    public class PassengerDTO
    {
        public PassengerDTO(string nationalCode, CityDTO city, float height, float weight, string emergencyContact, string emergencyPhone,
            Guid medicalDocumentFileId, Guid logBookDocumentFileId, Guid attorneyDocumentFileId, Guid nationalCardDocumentFileId)
        {
            NationalCode = nationalCode;
            City = city;
            Height = height;
            Weight = weight;
            EmergencyContact = emergencyContact;
            EmergencyPhone = emergencyPhone;
            MedicalDocumentFileId = medicalDocumentFileId;
            LogBookDocumentFileId = logBookDocumentFileId;
            AttorneyDocumentFileId = attorneyDocumentFileId;
            NationalCardDocumentFileId = nationalCardDocumentFileId;
        }

        public string NationalCode { get; set; }

        public CityDTO City { get; set; }

        public float Height { get; set; }

        public float Weight { get; set; }

        public string EmergencyContact { get; set; }

        public string EmergencyPhone { get; set; }

        public Guid MedicalDocumentFileId { get; set; }

        public Guid LogBookDocumentFileId { get; set; }

        public Guid AttorneyDocumentFileId { get; set; }

        public Guid NationalCardDocumentFileId { get; set; }
    }
}
