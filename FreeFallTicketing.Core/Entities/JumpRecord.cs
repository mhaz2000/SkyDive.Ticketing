using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    /// <summary>
    /// سوابق پرش
    /// </summary>
    public class JumpRecord : BaseEntity
    {
        public JumpRecord()
        {

        }
        public JumpRecord(DateTime date, string location, string equipments, string planeType, float height, DateTime time, string description, User user) : base()
        {
            Date = date;
            Location = location;
            Equipments = equipments;
            PlaneType = planeType;
            Height = height;
            Time = time;
            Description = description;
            User = user;
            Confirmed = false;
        }

        /// <summary>
        /// تاریخ پرش
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// محل پرش
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// تجهیزات
        /// </summary>
        public string Equipments { get; set; }

        /// <summary>
        /// نوع هواپیما
        /// </summary>
        public string PlaneType { get; set; }

        /// <summary>
        /// ارتفاع
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// مدت
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// وضعیت
        /// </summary>
        public bool Confirmed { get; private set; }

        /// <summary>
        /// شخصی که پرش کرده است.
        /// </summary>
        public User User { get; set; }

        public void SetAsConfirmd() => Confirmed = true;

        public void SetAsExpired() => Confirmed = false;
    }
}
