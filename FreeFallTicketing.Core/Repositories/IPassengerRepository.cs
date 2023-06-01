using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IPassengerRepository : IRepository<Passenger>
    {
        void AddNationalCardDocument(Passenger passenger, Guid fileId);
        void AddAttorneyDocument(Passenger passenger, Guid fileId, DateTime? expirationDate);
        void AddLogBookDocument(Passenger passenger, Guid fileId);
        void AddMedicalDocument(Passenger passenger, Guid fileId, DateTime? expirationDate);
        void ChangeDocumentStatus(PassengerDocument document, DocumentStatus status);
    }
}
