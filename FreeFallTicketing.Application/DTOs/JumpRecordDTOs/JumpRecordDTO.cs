namespace SkyDiveTicketing.Application.DTOs.JumpRecordDTOs
{
    public class JumpRecordDTO : BaseDTO<Guid>
    {
        public JumpRecordDTO(Guid id, DateTime createdAt, DateTime updatedAt, DateTime date, string location, string equipments, string planeType,
            float height, TimeOnly time, string description, bool confirmed) : base(id, createdAt, updatedAt)
        {
            Confirmed = confirmed;
            Date = date;
            Location = location;
            Equipments = equipments;
            PlaneType = planeType;
            Height = height;
            Time = time;
            Description = description;
        }

        public DateTime Date { get; set; }
        public string? Location { get; set; }
        public string? Equipments { get; set; }
        public string? PlaneType { get; set; }
        public float Height { get; set; }
        public TimeOnly Time { get; set; }
        public string? Description { get; set; }
        public bool Confirmed { get; set; }
    }
}
