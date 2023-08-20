namespace SkyDiveTicketing.Application.DTOs.UserDTOs
{
    public class TinyUserDto
    {
        public TinyUserDto(Guid id, int code, string fullName)
        {
            Id = id;
            Code = code;
            FullName = fullName;
        }

        public Guid Id { get; set; }
        public int Code { get; set; }
        public string FullName { get; set; }
    }
}
