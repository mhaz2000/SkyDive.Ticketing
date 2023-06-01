using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class UserTypeRepository : Repository<UserType>, IUserTypeRepository
    {
        public UserTypeRepository(DataContext context) : base(context)
        {
        }

        public async Task Create(string title)
        {
            await Context.UserTypes.AddAsync(new UserType(title));
        }

        public void Update(UserType userType, string title)
        {
            userType.Title= title;
            userType.UpdatedAt= DateTime.Now;
        }
    }
}
