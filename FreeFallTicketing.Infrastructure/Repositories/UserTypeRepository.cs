using Microsoft.EntityFrameworkCore;
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

        public async Task AddTicketType(SkyDiveEventTicketType ticketType, UserType userType)
        {
            if (!userType.AllowedTicketTypes.Any(c => c.TicketTypeId == ticketType.Id))
            {
                var entity = new UserTypeTicketType()
                {
                    TicketTypeId = ticketType.Id,
                    TicketType = ticketType,
                    UserType = userType,
                    UserTypeId = userType.Id
                };

                await Context.UserTypeTicketTypes.AddAsync(entity);

                userType.AllowedTicketTypes.Add(entity);
                ticketType.AllowedUserTypes.Add(entity);
            }
        }

        public async Task Create(string title)
        {
            await Context.UserTypes.AddAsync(new UserType(title));
        }

        public IQueryable<UserType> FindUserType()
        {
            return Context.UserTypes.Include(c => c.AllowedTicketTypes).ThenInclude(c => c.TicketType);
        }

        public async Task RemoveTicketType(SkyDiveEventTicketType ticketType, UserType userType)
        {
            if (userType.AllowedTicketTypes.Any(c => c.TicketTypeId == ticketType.Id))
            {
                var userTypeTicketType = await Context.UserTypeTicketTypes.FirstOrDefaultAsync(c => c.UserType == userType && c.TicketType == ticketType);
                ArgumentNullException.ThrowIfNull(userTypeTicketType);

                userType.AllowedTicketTypes.Remove(userTypeTicketType);
                ticketType.AllowedUserTypes.Remove(userTypeTicketType);

                Context.UserTypeTicketTypes.Remove(userTypeTicketType);
            }
        }

        public void Update(UserType userType, string title)
        {
            userType.Title = title;
            userType.UpdatedAt = DateTime.Now;
        }
    }
}
