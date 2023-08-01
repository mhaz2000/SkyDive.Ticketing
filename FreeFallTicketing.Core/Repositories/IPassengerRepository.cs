using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IPassengerRepository : IRepository<Passenger>
    {
        Task AddNationalCardDocument(Passenger passenger, Guid fileId);
        Task AddAttorneyDocumentAsync(Passenger passenger, Guid fileId, DateTime? expirationDate);
        Task AddLogBookDocumentAsync(Passenger passenger, Guid fileId);
        Task AddMedicalDocumentAsync(Passenger passenger, Guid fileId, DateTime? expirationDate);
    }
}
