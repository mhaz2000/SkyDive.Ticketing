namespace SkyDiveTicketing.Application.DTOs.UserDTOs
{
    public class UserTypeDTO : BaseDTO<Guid>
    {
        public UserTypeDTO(string title, bool isDefault, Guid id, DateTime createdAt, DateTime updatedAt, IEnumerable<UserTypeAllowedTicketTypeDTO> allowedTicketTypes)
            : base(id, createdAt, updatedAt)
        {
            Title = title;
            IsDefault = isDefault;
            AllowedTicketTypes = allowedTicketTypes;
        }

        public bool IsDefault { get; set; }
        public string Title { get; set; }

        public IEnumerable<UserTypeAllowedTicketTypeDTO> AllowedTicketTypes { get; set; }
    }

    public class UserTypeAllowedTicketTypeDTO : BaseDTO<Guid>
    {
        public UserTypeAllowedTicketTypeDTO(Guid id, DateTime createdAt, DateTime updatedAt, string title) : base(id, createdAt, updatedAt)
        {
            Title = title;
        }

        public string Title { get; set; }
    }
}
