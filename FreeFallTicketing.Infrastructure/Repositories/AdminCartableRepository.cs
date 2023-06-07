using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class AdminCartableRepository : Repository<AdminCartable>, IAdminCartableRepository
    {
        public AdminCartableRepository(DataContext context) : base(context)
        {
        }

        public async Task AddToCartable(string title, User applicant, RequestType type, Ticket? ticket = null)
        {
            var adminCartableRequest = new AdminCartable(title, applicant, type);

            if (type == RequestType.TicketCancellation && ticket is not null)
                ticket.SetRequest(adminCartableRequest);

            await Context.AdminCartables.AddAsync(adminCartableRequest);
        }

        public void SetAsDone(AdminCartable adminCartable)
        {
            adminCartable.SetAsDone();
        }
    }
}
