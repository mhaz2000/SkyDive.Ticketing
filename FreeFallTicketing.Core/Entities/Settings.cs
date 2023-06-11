using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class Settings : BaseEntity
    {
        public Settings() : base()
        {
            UserStatusInfo = new List<UserStatusInfo>();
        }

        public string? TermsAndConditionsUrl { get; set; }
        public float VAT { get; set; }
        public int JumpDuration { get; set; }
        public ICollection<UserStatusInfo> UserStatusInfo { get; set; }

        public void SetStatus(UserStatus status, string description)
        {
            var userstatusInfo = UserStatusInfo.FirstOrDefault(c => c.UserStatus == status);
            if (userstatusInfo is null)
                UserStatusInfo.Add(new UserStatusInfo(status, description));
            else
                userstatusInfo.Description = description;
        }

    }

    public class UserStatusInfo : BaseEntity
    {
        public UserStatusInfo()
        {

        }

        public UserStatusInfo(UserStatus userStatus, string description) : base() 
        {
            UserStatus = userStatus;
            Description = description;
        }

        public UserStatus UserStatus { get; set; }
        public string Description { get; set; }
    }
}
