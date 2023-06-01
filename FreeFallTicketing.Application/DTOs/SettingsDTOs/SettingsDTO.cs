using SkyDiveTicketing.Core.Entities;

namespace SkyDiveTicketing.Application.DTOs.SettingsDTOs
{
    public class SettingsDTO : BaseDTO<Guid>
    {
        public SettingsDTO(Guid id, DateTime createdAt, DateTime updatedAt, IEnumerable<UserStatusInfoDTO> userStatusInfo, string url)
            : base(id, createdAt, updatedAt)
        {
            UserStatusInfo = userStatusInfo;
            TermsAndConditionsUrl = url;
        }

        public string TermsAndConditionsUrl { get; set; }

        public IEnumerable<UserStatusInfoDTO> UserStatusInfo { get; set; }

    }

    public class UserStatusInfoDTO
    {
        public UserStatusInfoDTO(UserStatus status, string description)
        {
            Status = status;
            Description = description;
        }

        public UserStatus Status { get; set; }
        public string Description { get; set; }
    }
}
