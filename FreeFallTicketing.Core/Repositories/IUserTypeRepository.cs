using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IUserTypeRepository : IRepository<UserType>
    {
        IQueryable<UserType> FindUserType();
        void Update(UserType userType, string title);
        Task Create(string title);
        Task AddTicketType(SkyDiveEventTicketType ticketType, UserType userType);
        Task RemoveTicketType(SkyDiveEventTicketType ticketType, UserType userType);
    }
}
