using SkyDiveTicketing.Core.Entities;

namespace SkyDiveTicketing.Application.DTOs.SettingsDTOs
{
    public class SettingsDTO : BaseDTO<Guid>
    {
        public SettingsDTO(Guid id, DateTime createdAt, DateTime updatedAt, IEnumerable<UserStatusInfoDTO> userStatusInfo, string url, string registrationUrl,
            int jumpDuration, int fileSizeLimitation, int attorneyDocumentsValidityDuration, int medicalDocumentsValidityDuration) : base(id, createdAt, updatedAt)
        {
            UserStatusInfo = userStatusInfo;
            TermsAndConditionsUrl = url;
            JumpDuration = jumpDuration;
            RegistrationTermsAndConditionsUrl = registrationUrl;
            FileSizeLimitation = fileSizeLimitation;
            MedicalDocumentsValidityDuration = medicalDocumentsValidityDuration;
            AttorneyDocumentsValidityDuration= attorneyDocumentsValidityDuration;
        }

        public int FileSizeLimitation { get; set; }
        public string TermsAndConditionsUrl { get; set; }
        public string RegistrationTermsAndConditionsUrl { get; set; }
        public int JumpDuration { get; set; }
        public int MedicalDocumentsValidityDuration { get; set; }
        public int AttorneyDocumentsValidityDuration { get; set; }

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
