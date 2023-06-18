using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class SkyDiveEventStatus : BaseEntity
    {
        public SkyDiveEventStatus()
        {

        }

        public SkyDiveEventStatus(string title, bool reservable) : base()
        {
            Title = title;
            Reservable = reservable;
        }

        public string Title { get; set; }
        public bool Reservable { get; set; }
    }
}
