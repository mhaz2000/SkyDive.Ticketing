using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;

namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class FlightLoadCancellationTypeRepository : Repository<FlightLoadCancellationType>, IFlightLoadCancellationTypeRepository
    {
        public FlightLoadCancellationTypeRepository(DataContext context) : base(context)
        {
        }

        public void UpdateFlightLoadCancellationType(FlightLoad flightLoad, IDictionary<int, float> cancellationTypes)
        {
            foreach (var (hoursBeforeCancellation, rate) in cancellationTypes)
            {
                //flightLoad.AddCancellationTypes(new FlightLoadCancellationType(flightLoad, hoursBeforeCancellation, rate));
            }
        }

        public void UpdateFlightLoadCancellationType(FlightLoadCancellationType cancellationType, int hoursBeforeCancellation, float rate)
        {
            cancellationType.Rate = rate;
            cancellationType.HoursBeforeCancellation = hoursBeforeCancellation;
        }
    }
}
