using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class FlightLoadCancellationType : BaseEntity
    {
        public FlightLoadCancellationType()
        {

        }

        public FlightLoadCancellationType(FlightLoad flightLoad, int hoursBeforeCancellation, float rate) : base()
        {
            FlightLoad = flightLoad;
            HoursBeforeCancellation = hoursBeforeCancellation;
            Rate = rate;
        }
        public FlightLoad FlightLoad { get; set; }
        public int HoursBeforeCancellation { get; set; }
        public string Title { get; set; }
        public float Rate { get; set; }
    }
}
