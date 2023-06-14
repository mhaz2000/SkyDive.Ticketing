using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class PassengerRepository : Repository<Passenger>, IPassengerRepository
    {
        public PassengerRepository(DataContext context) : base(context)
        {
        }

        public void AddAttorneyDocument(Passenger passenger, Guid fileId, DateTime? expirationDate)
        {
            passenger.AttorneyDocumentFile?.Upload(fileId, expirationDate);
        }

        public void AddLogBookDocument(Passenger passenger, Guid fileId)
        {
            passenger.LogBookDocumentFile?.Upload(fileId, null);
        }

        public void AddMedicalDocument(Passenger passenger, Guid fileId, DateTime? expirationDate)
        {
            passenger.MedicalDocumentFile?.Upload(fileId, expirationDate);
        }

        public void AddNationalCardDocument(Passenger passenger, Guid fileId)
        {
            if (passenger.NationalCardDocumentFile.Status != DocumentStatus.Confirmed)
                passenger.NationalCardDocumentFile?.Upload(fileId, null);
        }
    }
}
