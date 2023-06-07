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
        }

        public string Title { get; set; }
        public bool IsDefault { get; set; }
    }
}
