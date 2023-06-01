using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        public void AddMessage(User user, string message)
        {
            user.AddMessage(new Message(message));
        }

        public void ChangeUserStatus(User user, UserStatus status)
        {
            user.Status = status;
        }

        public void CheckPersonalInformation(User user, bool isConfirmed)
        {
            if (isConfirmed)
                user.Status = UserStatus.Active;
            else
                user.Status = UserStatus.AwaitingCompletion;
        }

        public void CompeleteOtherUserPersonalInfo(string email, DefaultCity? city, string address, string emergencyContact, string emergencyPhone, float? height, float? weight, User user)
        {
            user.Passenger = user.Passenger ?? new Passenger(user.NationalCode, city, address, height, weight, emergencyContact, emergencyPhone);
            user.Email = email;
        }

        public void CompeleteUserPersonalInfo(string firstName, string lastName, string nationalCode, DateTime birthDate, User user)
        {
            if (!user.PersonalInformationIsCompeleted)
            {
                user.FirstName = firstName;
                user.LastName = lastName;
                user.NationalCode = nationalCode;
                user.BirthDate = birthDate;
                user.Status = UserStatus.AwaitingCompletion;
                user.PersonalInformationIsCompeleted = true;
            }
        }

        public void CompeleteUserSecurityInfo(string username, string password, User user)
        {
            var _passwordHasher = new PasswordHasher<User>();

            user.UserName = username;
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
        }

        public async Task<string> CreateAsync(string phone)
        {
            var defaultType = await Context.UserTypes.FirstOrDefaultAsync(c=> c.IsDefault);
            var user = new User()
            {
                Status = UserStatus.AwaitingCompletion,
                PhoneNumber = phone,
                SecurityStamp = Guid.NewGuid().ToString(),
                Code = await GetUserCode(),
                UserType = defaultType
            };

            await Context.Users.AddAsync(user);

            var role = Context.Roles.FirstOrDefault(c => c.Name == "User");
            await Context.UserRoles.AddAsync(new IdentityUserRole<string>() { RoleId = role.Id, UserId = user.Id });

            return user.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var user = await Context.Users.FindAsync(id);

            Context.Users.Remove(user);
        }

        public async Task EditAsync(string username, string email, string firstName, string lastName, string phone, string password, string userId)
        {
            var _passwordHasher = new PasswordHasher<User>();

            var user = await Context.Users.FindAsync(userId);

            user.PhoneNumber = phone;
            user.FirstName = firstName ?? "";
            user.LastName = lastName ?? "";
            user.Email = email;
            user.UserName = username;

            if (!string.IsNullOrEmpty(password))
                user.PasswordHash = _passwordHasher.HashPassword(user, password);
        }

        public IQueryable<User> GetAllWithIncludes(Expression<Func<User, bool>> predicate)
        {
            return Context.Users
                .Include(c => c.Passenger).ThenInclude(c => c.NationalCardDocumentFile)
                .Include(c => c.Passenger).ThenInclude(c => c.AttorneyDocumentFile)
                .Include(c => c.Passenger).ThenInclude(c => c.MedicalDocumentFile)
                .Include(c => c.Passenger).ThenInclude(c => c.LogBookDocumentFile)
                .Where(predicate);
        }

        public async Task<User?> GetUserAsync(string id)
        {
            return await Context.Users.FindAsync(id) ?? null;
        }

        public void InactivateUser(User user)
        {
            user.Status= UserStatus.Inactive;
        }

        public void LoginFailed(User user)
        {
            user.LoginFailedAttempts++;
        }

        public void LoginSucceeded(User user)
        {
            user.LoginFailedAttempts = 0;
        }

        public void PhoneConfirmed(User user)
        {
            user.PhoneNumberConfirmed = true;
        }

        public void SetOtpCode(User user, string otpCode)
        {
            user.OtpCode = otpCode;
            user.OtpRequestTime = DateTime.Now;
        }

        public void UpdateUserPassword(string password, User user)
        {
            var _passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
        }

        private async Task<int> GetUserCode()
        {
            var code = (await Context.Users.OrderByDescending(c => c.Code).FirstOrDefaultAsync())?.Code ?? 100000;
            return code++;
        }
    }
}
