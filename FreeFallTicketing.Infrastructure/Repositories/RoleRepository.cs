﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class RoleRepository : Repository<IdentityRole<Guid>>, IRoleRepository
    {
        public RoleRepository(DataContext context) : base(context)
        {
        }

        public IEnumerable<Guid> GetAdminUsers()
        {
            var adminRole = Context.Roles.FirstOrDefault(c=> c.Name == "Admin");
            return Context.UserRoles.Where(c => c.RoleId == adminRole.Id).Select(c=> c.UserId);
        }

        public IQueryable<IdentityRole<Guid>> GetUserRoles(User user)
        {
            var roles = Context.UserRoles.Where(c => c.UserId == user.Id).Select(s=> s.RoleId);
            return Context.Roles.Where(c=> roles.Contains(c.Id));
        }
    }
}
