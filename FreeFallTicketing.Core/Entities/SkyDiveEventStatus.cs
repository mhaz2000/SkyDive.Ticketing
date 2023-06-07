using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class SkyDiveEventStatus : BaseEntity
    {
        public SkyDiveEventStatus()
        {

        }

        public SkyDiveEventStatus(string title) : base()
        {
            Title = title;
        }

        public string Title { get; set; }
    }
}
