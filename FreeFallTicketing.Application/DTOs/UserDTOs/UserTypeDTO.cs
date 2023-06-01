namespace SkyDiveTicketing.Application.DTOs.UserDTOs
{
    public class UserTypeDTO : BaseDTO<Guid>
    {
        public UserTypeDTO(string title, bool isDefault, Guid id, DateTime createdAt, DateTime updatedAt) : base(id, createdAt, updatedAt)
        {
            Title = title;
            IsDefault= isDefault;
        }

        public bool IsDefault { get; set; }
        public string Title { get; set; }
    }
}
