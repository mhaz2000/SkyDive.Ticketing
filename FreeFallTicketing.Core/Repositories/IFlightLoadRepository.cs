using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IFlightLoadRepository : IRepository<FlightLoad>
    {
        Task<FlightLoad> CreateFlightLoad(string number, DateTime date);

        //Task AddSeat(FlightLoad flightLoad, )

        void UpdateFlightLoad(string number, DateTime date, int type1SeatNumber, int type2SeatNumber, int type3SeatNumber, double type1SeatAmount, double type2SeatAmount, double type3SeatAmount, FlightLoad flightLoad);
        void RemoveFlightLoadCancellationType(FlightLoad flightLoad, FlightLoadCancellationType cancellationType);
    }
}
