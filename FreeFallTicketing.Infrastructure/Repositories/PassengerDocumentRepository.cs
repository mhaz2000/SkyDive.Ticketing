using Microsoft.EntityFrameworkCore;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class PassengerMedicalDocumentRepository : Repository<MedicalDocument>, IPassengerMedicalDocumentRepository
    {
        public PassengerMedicalDocumentRepository(DataContext context) : base(context)
        {
        }

        public async Task ExpireDocuments()
        {
            var documents = Context.MedicalDocuments.Where(c => c.ExpirationDate != null && c.ExpirationDate < DateTime.Now);
            var users = Context.Users
                .Include(c => c.Messages)
                .Include(c => c.Passenger).ThenInclude(c => c.MedicalDocumentFile);

            foreach (var document in documents)
            {
                document.SetStatus(DocumentStatus.Expired);

                var user = await users.FirstOrDefaultAsync(c => c.Passenger.MedicalDocumentFile == document);
                if (user is not null)
                {
                    user.Status = UserStatus.Pending;
                    user.AddMessage(new Message($"{user.FirstName} {user.LastName} عزیز مدارک پزشکی شما منقضی شده است. لطفا اقدامات لازم جهت بارگذاری مدارک پزشکی جدید را انجام دهید."));
                }
            }
        }
    }

    public class PassengerAttorneyDocumentRepository : Repository<AttorneyDocument>, IPassengerAttorneyDocumentRepository
    {
        public PassengerAttorneyDocumentRepository(DataContext context) : base(context)
        {
        }

        public async Task ExpireDocuments()
        {
            var documents = Context.AttorneyDocuments.Where(c => c.ExpirationDate != null && c.ExpirationDate < DateTime.Now);
            var users = Context.Users
                .Include(c => c.Messages)
                .Include(c => c.Passenger).ThenInclude(c => c.AttorneyDocumentFile);

            foreach (var document in documents)
            {
                document.SetStatus(DocumentStatus.Expired);
                var user = await users.FirstOrDefaultAsync(c => c.Passenger.AttorneyDocumentFile == document);
                if (user is not null)
                {
                    user.Status = UserStatus.Pending;
                    user.AddMessage(new Message($"{user.FirstName} {user.LastName} عزیز وکالتنامه محضری شما منقضی شده است. لطفا اقدامات لازم جهت تمدید وکالتنامه را انجام دهید."));
                }
            }
        }
    }

    public class PassengerNationalCardDocumentRepository : Repository<NationalCardDocument>, IPassengerNationalCardDocumentRepository
    {
        public PassengerNationalCardDocumentRepository(DataContext context) : base(context)
        {
        }
    }

    public class PassengerLogBookDocumentRepository : Repository<LogBookDocument>, IPassengerLogBookDocumentRepository
    {
        public PassengerLogBookDocumentRepository(DataContext context) : base(context)
        {
        }
    }
}
