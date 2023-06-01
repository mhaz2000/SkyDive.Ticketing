using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IFlightLoadCancellationTypeRepository : IRepository<FlightLoadCancellationType>
    {
        void UpdateFlightLoadCancellationType(FlightLoad flightLoad, IDictionary<int, float> cancellationTypes);

        void UpdateFlightLoadCancellationType(FlightLoadCancellationType cancellationType, int hoursBeforeCancellation, float rate);
    }
}
