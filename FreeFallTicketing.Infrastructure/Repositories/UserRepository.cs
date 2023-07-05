using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Data;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        public async Task AddMessage(User user, string message, string title)
        {
            var entity = new Message(message, title);
            await Context.Messages.AddAsync(entity);
            user.AddMessage(entity);
        }

        public void AssignUserType(User user, UserType userType)
        {
            user.UserType = userType;
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

        public void CompeleteOtherUserPersonalInfo(string email, string? state, string? city, string address, string emergencyContact, string emergencyPhone, float? height,
            float? weight, User user)
        {
            if (user.Passenger is not null)
            {
                user.Passenger.City = city;
                user.Passenger.Address = address;
                user.Passenger.Height = height;
                user.Passenger.Weight = weight;
                user.Passenger.EmergencyContact = emergencyContact;
                user.Passenger.EmergencyPhone = emergencyPhone;
                user.Passenger.NationalCode = user.NationalCode;
            }
            else
            {
                var entity = new Passenger(user.NationalCode, state, city, address, height, weight, emergencyContact, emergencyPhone);
                Context.Passengers.Add(entity);
                user.Passenger = entity;
            }

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

        public async Task<Guid> CreateAsync(string phone)
        {
            var defaultType = await Context.UserTypes.FirstOrDefaultAsync(c => c.IsDefault);
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
            await Context.UserRoles.AddAsync(new IdentityUserRole<Guid>() { RoleId = role.Id, UserId = user.Id });
            await Context.Wallets.AddAsync(new Wallet(0, user));

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
            user.Status = UserStatus.Inactive;
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
            var lastCode = Context.Users.OrderByDescending(c => c.Code).FirstOrDefault()?.Code;

            user.Code = lastCode is not null && lastCode != 0 ? lastCode.Value + 1 : 100001;
            user.PhoneNumberConfirmed = true;
        }

        public void SetOtpCode(User user, string otpCode)
        {
            user.OtpCode = otpCode;
            user.OtpRequestTime = DateTime.Now;
        }

        public void UpdateUser(User user, float? weight, float? height, string state, string city, string? lastName, string? firstName, string? nationalCode, string? emergencyPhone,
            string? address, DateTime? birthDate, string? emergencyContact, string email, string phone, string username)
        {
            user.Passenger.Weight = weight;
            user.Passenger.Height = height;
            user.Passenger.City = city;
            user.LastName = lastName;
            user.FirstName = firstName;
            user.NationalCode = nationalCode;
            user.Passenger.EmergencyContact = emergencyContact;
            user.Passenger.EmergencyPhone = emergencyPhone;
            user.Passenger.Address = address;
            user.Email = email;
            user.PhoneNumber = phone;
            user.BirthDate = birthDate;
            user.UserName = username;
        }

        public void UpdateUserPassword(string password, User user)
        {
            var _passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
        }

        private async Task<int> GetUserCode()
        {
            var code = (await Context.Users.OrderByDescending(c => c.Code).FirstOrDefaultAsync())?.Code ?? 100000;
            return ++code;
        }

        public void ResetFailedAttempts(User user)
        {
            user.LoginFailedAttempts = 0;
        }

        public async Task<User> AddUser(string password, string? nationalCode, float? height, float? weight, string? firstName, string? lastName, string? email,
            DateTime? birthDate, string? phone, string? username, string? address, string? emergencyContact, string? emergencyPhone, string? state, string? city)
        {
            var defaultType = await Context.UserTypes.FirstOrDefaultAsync(c => c.IsDefault);
            var user = new User()
            {
                Status = UserStatus.AwaitingCompletion,
                PhoneNumber = phone,
                SecurityStamp = Guid.NewGuid().ToString(),
                Code = await GetUserCode(),
                UserType = defaultType,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                NationalCode = nationalCode,
                BirthDate = birthDate,
                PhoneNumberConfirmed = true,
                UserName = username
            };

            var passenger = new Passenger(nationalCode, state, city, address, height, weight, emergencyContact, emergencyPhone);
            await Context.Passengers.AddAsync(passenger);

            user.Passenger = passenger;

            await Context.Users.AddAsync(user);

            var role = Context.Roles.FirstOrDefault(c => c.Name == "User");
            await Context.UserRoles.AddAsync(new IdentityUserRole<Guid>() { RoleId = role.Id, UserId = user.Id });
            await Context.Wallets.AddAsync(new Wallet(0, user));

            var _passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = _passwordHasher.HashPassword(user, password);

            return user;
        }

        public void AcceptingTermsAndConditions(User user)
        {
            user.TermsAndConditionsAcceptance = true;
        }

        public async Task<User?> GetUserWithInclude(Expression<Func<User, bool>> filter)
        {
            return await Context.Users
                .Include(c => c.Passenger).ThenInclude(c => c.City)
                .Include(c => c.UserType)
                .ThenInclude(c => c!.AllowedTicketTypes).ThenInclude(c => c.TicketType).FirstOrDefaultAsync(filter);
        }
    }
}
