using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IPassengerLogBookDocumentRepository : IRepository<LogBookDocument>
    {
    }

    public interface IPassengerMedicalDocumentRepository : IRepository<MedicalDocument>
    {
        Task ExpireDocuments();
    }

    public interface IPassengerAttorneyDocumentRepository : IRepository<AttorneyDocument>
    {
        Task ExpireDocuments();
    }

    public interface IPassengerNationalCardDocumentRepository : IRepository<NationalCardDocument>
    {
    }
}
