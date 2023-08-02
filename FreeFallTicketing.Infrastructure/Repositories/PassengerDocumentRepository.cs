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

        public void ChangeUserStatusIfNeeded(MedicalDocument document, User? user)
        {

            if (user is null)
                return;

            if (Context.MedicalDocuments.OrderByDescending(c => c.CreatedAt).FirstOrDefault() == document)
                user.Status = UserStatus.AwaitingCompletion;
        }

        public async Task ExpireDocuments()
        {
            var documents = Context.MedicalDocuments.Where(c => c.ExpirationDate != null && c.ExpirationDate < DateTime.Now);
            var users = Context.Users
                .Include(c => c.Messages)
                .Include(c => c.Passenger).ThenInclude(c => c.MedicalDocumentFiles);

            foreach (var document in documents)
            {
                document.SetStatus(DocumentStatus.Expired);

                var user = await users.FirstOrDefaultAsync(c => c.Passenger.MedicalDocumentFiles.Any(file => file == document && file.Status == DocumentStatus.Confirmed));
                if (user is not null)
                {
                    user.Status = UserStatus.Pending;
                    user.AddMessage(new Message($"{user.FirstName} {user.LastName} عزیز مدارک پزشکی شما منقضی شده است. لطفا اقدامات لازم جهت بارگذاری مدارک پزشکی جدید را انجام دهید.", "سر رسید تاریخ انقضای مدرک"));
                }
            }
        }
    }

    public class PassengerAttorneyDocumentRepository : Repository<AttorneyDocument>, IPassengerAttorneyDocumentRepository
    {
        public PassengerAttorneyDocumentRepository(DataContext context) : base(context)
        {
        }

        public void ChangeUserStatusIfNeeded(AttorneyDocument document, User? user)
        {

            if (user is null)
                return;

            if (Context.AttorneyDocuments.OrderByDescending(c => c.CreatedAt).FirstOrDefault() == document)
                user.Status = UserStatus.AwaitingCompletion;
        }

        public async Task ExpireDocuments()
        {
            var documents = Context.AttorneyDocuments.Where(c => c.ExpirationDate != null && c.ExpirationDate < DateTime.Now);
            var users = Context.Users
                .Include(c => c.Messages)
                .Include(c => c.Passenger).ThenInclude(c => c.AttorneyDocumentFiles);

            foreach (var document in documents)
            {
                var user = await users.FirstOrDefaultAsync(c => c.Passenger.AttorneyDocumentFiles.Any(file => file == document && file.Status == DocumentStatus.Confirmed));
                document.SetStatus(DocumentStatus.Expired);
                if (user is not null)
                {
                    user.Status = UserStatus.Pending;
                    user.AddMessage(new Message($"{user.FirstName} {user.LastName} عزیز وکالتنامه محضری شما منقضی شده است. لطفا اقدامات لازم جهت تمدید وکالتنامه را انجام دهید.", "سر رسید تاریخ انقضای مدرک"));
                }
            }
        }
    }

    public class PassengerNationalCardDocumentRepository : Repository<NationalCardDocument>, IPassengerNationalCardDocumentRepository
    {
        public PassengerNationalCardDocumentRepository(DataContext context) : base(context)
        {
        }

        public void ChangeUserStatusIfNeeded(NationalCardDocument document, User? user)
        {

            if (user is null)
                return;

            if (Context.NationalCardDocuments.OrderByDescending(c => c.CreatedAt).FirstOrDefault() == document)
                user.Status = UserStatus.AwaitingCompletion;
        }
    }

    public class PassengerLogBookDocumentRepository : Repository<LogBookDocument>, IPassengerLogBookDocumentRepository
    {

        public PassengerLogBookDocumentRepository(DataContext context) : base(context)
        {
        }

        public void ChangeUserStatusIfNeeded(LogBookDocument document, User? user)
        {
            if (user is null)
                return;

            if(Context.LogBookDocuments.OrderByDescending(c => c.CreatedAt).FirstOrDefault() == document)
                user.Status = UserStatus.AwaitingCompletion;

        }
    }
}
