using Microsoft.EntityFrameworkCore;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;
using System.Linq.Expressions;

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

        public IQueryable<AdminCartable> GetAdminCartables(Expression<Func<AdminCartable, bool>>? predicate)
        {
            var cartables = Context.AdminCartables.Include(c => c.Applicant).ThenInclude(c => c.UserType).AsQueryable();

            if (predicate is not null)
                cartables = cartables.Where(predicate);

            return cartables;
        }

        public void SetAsDone(AdminCartable adminCartable)
        {
            adminCartable.SetAsDone();
        }
    }
}
