using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class UserType : BaseEntity
    {
        public UserType()
        {

        }

        public UserType(string title) : base()
        {
            Title = title;
            IsDefault = false;
            AllowedTicketTypes = new List<SkyDiveEventTicketType>();
        }

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// نوع دیفالت
        /// </summary>
        public bool IsDefault { get; set; }
       
        /// <summary>
        /// کاربر قابلیت رزرو چه بلیط هایی را داشته باشد.
        /// </summary>
        public ICollection<SkyDiveEventTicketType> AllowedTicketTypes { get; set; }
    }
}
