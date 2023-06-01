using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;
using System.Linq.Expressions;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        IQueryable<User> GetAllWithIncludes(Expression<Func<User, bool>> predicate);

        Task<User?> GetUserAsync(string id);

        Task<string> CreateAsync(string phone);

        Task EditAsync(string username, string email, string firstName, string lastName, string phone, string password, string userId);

        Task DeleteAsync(string id);

        void SetOtpCode(User user, string otpCode);

        void ChangeUserStatus(User user, UserStatus status);

        void CompeleteUserPersonalInfo(string firstName, string lastName, string nationalCode, DateTime birthDate, User user);

        void CompeleteOtherUserPersonalInfo(string email, DefaultCity? city, string address, string emergencyContact, string emergencyPhone, float? height, float? weight, User user);

        void CompeleteUserSecurityInfo(string username, string password, User user);

        void UpdateUserPassword(string password, User user);

        void PhoneConfirmed(User user);

        void InactivateUser(User user);

        void CheckPersonalInformation(User user, bool isConfirmed);

        void LoginSucceeded(User user);

        void LoginFailed(User user);

        void AddMessage(User user, string message);
    }
}
