﻿namespace SkyDiveTicketing.Application.DTOs.JumpRecordDTOs
{
    public class JumpRecordDTO : BaseDTO<Guid>
    {
        public JumpRecordDTO(Guid id, DateTime createdAt, DateTime updatedAt, DateTime date, string location, string equipments, string planeType,
            float height, TimeSpan time, string description, bool confirmed, Guid userId) : base(id, createdAt, updatedAt)
        {
            Confirmed = confirmed;
            Date = date;
            Location = location;
            Equipments = equipments;
            PlaneType = planeType;
            Height = height;
            Time = time;
            Description = description;
            UserId = userId;
        }

        public DateTime Date { get; set; }
        public string? Location { get; set; }
        public string? Equipments { get; set; }
        public string? PlaneType { get; set; }
        public float Height { get; set; }
        public TimeSpan Time { get; set; }
        public string? Description { get; set; }
        public bool Confirmed { get; set; }
        public Guid UserId { get; set; }
    }
}
