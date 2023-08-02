using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class Settings : BaseEntity
    {
        public Settings() : base()
        {
            UserStatusInfo = new List<UserStatusInfo>();
        }

        public int FileSizeLimitiation { get; set; }
        public string? TermsAndConditionsUrl { get; set; }
        public string? RegistrationTermsAndConditionsUrl { get; set; }
        public float VAT { get; set; }
        public int JumpDuration { get; set; }
        public ICollection<UserStatusInfo> UserStatusInfo { get; set; }

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
