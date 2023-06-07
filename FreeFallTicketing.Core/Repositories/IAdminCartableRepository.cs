using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IAdminCartableRepository : IRepository<AdminCartable>
    {
        Task AddToCartable(string title, User applicant, RequestType type, Ticket? ticket = null);
        void SetAsDone(AdminCartable adminCartable);
    }
}
