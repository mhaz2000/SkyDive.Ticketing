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

        public async Task AddAttorneyDocumentAsync(Passenger passenger, Guid fileId, DateTime? expirationDate)
        {
            AttorneyDocument attorneyDocument = new AttorneyDocument();
            attorneyDocument.Upload(fileId, expirationDate);

            await Context.AttorneyDocuments.AddAsync(attorneyDocument);
            passenger.AttorneyDocumentFiles.Add(attorneyDocument);
        }

        public async Task AddLogBookDocumentAsync(Passenger passenger, Guid fileId)
        {
            LogBookDocument logBookDocument = new LogBookDocument();
            logBookDocument.Upload(fileId, null);

            await Context.LogBookDocuments.AddAsync(logBookDocument);
            passenger.LogBookDocumentFiles.Add(logBookDocument);
        }

        public async Task AddMedicalDocumentAsync(Passenger passenger, Guid fileId, DateTime? expirationDate)
        {
            MedicalDocument medicalDocument = new MedicalDocument();
            medicalDocument.Upload(fileId,expirationDate);

            await Context.MedicalDocuments.AddAsync(medicalDocument);
            passenger.MedicalDocumentFiles.Add(medicalDocument);
        }

        public async Task AddNationalCardDocument(Passenger passenger, Guid fileId)
        {
            if (passenger.NationalCardDocumentFiles.All(c => c.Status != DocumentStatus.Confirmed))
            {
                NationalCardDocument nationalCard = new NationalCardDocument();
                nationalCard.Upload(fileId, null);

                await Context.NationalCardDocuments.AddAsync(nationalCard);
                passenger.NationalCardDocumentFiles.Add(nationalCard);
            }
        }
    }
}
