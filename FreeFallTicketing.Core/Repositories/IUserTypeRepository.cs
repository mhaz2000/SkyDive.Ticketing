using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IUserTypeRepository : IRepository<UserType>
    {
        void Update(UserType userType, string title);
        Task Create(string title);
    }
}
