using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class Ticket : BaseEntity
    {
        public Ticket(FlightLoad flightLoad, int type1SeatReservedQuantity, int type2SeatReservedQuantity, int type3SeatReservedQuantity) : base()
        {
            FlightLoad = flightLoad;
            Type1SeatReservedQuantity = type1SeatReservedQuantity;
            Type2SeatReservedQuantity = type2SeatReservedQuantity;
            Type3SeatReservedQuantity = type3SeatReservedQuantity;

            Amount = (flightLoad.Type1SeatAmount * type1SeatReservedQuantity) +
                (flightLoad.Type2SeatAmount * type2SeatReservedQuantity) +
                (flightLoad.Type3SeatAmount * type3SeatReservedQuantity);

            Passengers = new List<Passenger>();

            Status = TicketStatus.Pending;

            ReserveTime = DateTime.Now;
        }

        public FlightLoad FlightLoad { get; set; }
        public double Amount { get; set; }
        public ICollection<Passenger> Passengers { get; set; }
        public int Type1SeatReservedQuantity { get; set; }
        public int Type2SeatReservedQuantity { get; set; }
        public int Type3SeatReservedQuantity { get; set; }

        public DateTime ReserveTime { get; set; }

        public TicketStatus Status { get; set; }

        public void AddPassenger(Passenger passenger)
        {
            Passengers.Add(passenger);
        }

        public void SetAsPaid() => Status = TicketStatus.Paid;

        public void SetAsExpired() => Status = TicketStatus.Expired;

        public void UpdateAmount() => Amount = (FlightLoad.Type1SeatAmount * Type1SeatReservedQuantity) + (FlightLoad.Type2SeatAmount * Type2SeatReservedQuantity)
            + (FlightLoad.Type3SeatAmount * Type3SeatReservedQuantity);
    }

    public enum TicketStatus
    {
        Paid,
        Expired,
        Pending
    }
}
