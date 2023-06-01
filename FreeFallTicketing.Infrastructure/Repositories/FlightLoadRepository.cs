using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories;
using SkyDiveTicketing.Infrastructure.Data;
using SkyDiveTicketing.Infrastructure.Repositories.Base;


namespace SkyDiveTicketing.Infrastructure.Repositories
{
    public class FlightLoadRepository : Repository<FlightLoad>, IFlightLoadRepository
    {
        public FlightLoadRepository(DataContext context) : base(context)
        {
        }

        public async Task<Guid> CreateFlightLoad(string number, DateTime date, int type1SeatNumber, int type2SeatNumber, int type3SeatNumber,
            double type1SeatAmount, double type2SeatAmount, double type3SeatAmount)
        {
            var entity = new FlightLoad(number, date, type1SeatNumber, type2SeatNumber, type3SeatNumber, type1SeatAmount, type2SeatAmount, type3SeatAmount);
            await Context.FlightLoads.AddAsync(entity);

            return entity.Id;
        }

        public void RemoveFlightLoadCancellationType(FlightLoad flightLoad, FlightLoadCancellationType cancellationType)
        {
            flightLoad.CancellationTypes.Remove(cancellationType);
        }

        public void UpdateFlightLoad(string number, DateTime date, int type1SeatNumber, int type2SeatNumber, int type3SeatNumber,
            double type1SeatAmount, double type2SeatAmount, double type3SeatAmount, FlightLoad flightLoad)
        {
            flightLoad.Date = date;
            flightLoad.Number = number;
            flightLoad.Type1SeatNumber = type1SeatNumber;
            flightLoad.Type2SeatNumber = type2SeatNumber;
            flightLoad.Type3SeatNumber = type3SeatNumber;
        }
    }
}
