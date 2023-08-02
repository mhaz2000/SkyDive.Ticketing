using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IPassengerLogBookDocumentRepository : IRepository<LogBookDocument>
    {
        void ChangeUserStatusIfNeeded(LogBookDocument document, User? user);
    }

    public interface IPassengerMedicalDocumentRepository : IRepository<MedicalDocument>
    {
        void ChangeUserStatusIfNeeded(MedicalDocument document, User? user);
        Task ExpireDocuments();
    }

    public interface IPassengerAttorneyDocumentRepository : IRepository<AttorneyDocument>
    {
        void ChangeUserStatusIfNeeded(AttorneyDocument document, User? user);
        Task ExpireDocuments();
    }

    public interface IPassengerNationalCardDocumentRepository : IRepository<NationalCardDocument>
    {
        void ChangeUserStatusIfNeeded(NationalCardDocument document, User? user);
    }
}
