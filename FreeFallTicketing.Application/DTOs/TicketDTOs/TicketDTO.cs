using SkyDiveTicketing.Core.Entities;

namespace SkyDiveTicketing.Application.DTOs.TicketDTOs
{
    public class TicketDTO : BaseDTO<Guid>
    {
        public TicketDTO(Guid id, DateTime createdAt, DateTime updatedAt, double amount,
            int type1SeatReservedQuantity, int type2SeatReservedQuantity, int type3SeatReservedQuantity,
            DateTime reserveTime, TicketStatus status, IEnumerable<PassengerDTO> passengers) : base(id, createdAt, updatedAt)
        {
            Amount = amount;
            Type1SeatReservedQuantity = type1SeatReservedQuantity;
            Type2SeatReservedQuantity = type2SeatReservedQuantity;
            Type3SeatReservedQuantity = type3SeatReservedQuantity;

            ReserveTime = reserveTime;
            Status = status;

            Passengers = passengers;
        }
        public double Amount { get; set; }
        public IEnumerable<PassengerDTO> Passengers { get; set; }
        public int Type1SeatReservedQuantity { get; set; }
        public int Type2SeatReservedQuantity { get; set; }
        public int Type3SeatReservedQuantity { get; set; }

        public DateTime ReserveTime { get; set; }
        public TicketStatus Status { get; set; }
    }
}
