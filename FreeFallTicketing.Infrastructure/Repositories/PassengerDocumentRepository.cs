using Microsoft.EntityFrameworkCore;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class PassengerDocumentRepository : Repository<PassengerDocument>, IPassengerDocumentRepository
    {
        public PassengerDocumentRepository(DataContext context) : base(context)
        {
        }

        public async Task ExpireDocuments()
        {
            var documents = Context.Documents.Where(c => c.ExpirationDate != null && c.ExpirationDate < DateTime.Now);
            var users = Context.Users
                .Include(c => c.Messages)
                .Include(c => c.Passenger).ThenInclude(c => c.MedicalDocumentFile)
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

                user = await users.FirstOrDefaultAsync(c => c.Passenger.MedicalDocumentFile == document);
                if (user is not null)
                {
                    user.Status = UserStatus.Pending;
                    user.AddMessage(new Message($"{user.FirstName} {user.LastName} عزیز مدارک پزشکی شما منقضی شده است. لطفا اقدامات لازم جهت بارگذاری مدارک پزشکی جدید را انجام دهید."));
                }
            }
        }
    }
}
