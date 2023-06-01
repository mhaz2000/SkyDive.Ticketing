using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IPassengerDocumentRepository : IRepository<PassengerDocument>
    {
        Task ExpireDocuments();
    }
}
