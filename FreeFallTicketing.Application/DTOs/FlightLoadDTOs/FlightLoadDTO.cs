namespace SkyDiveTicketing.Application.DTOs.FlightLoadDTOs
{
    public class FlightLoadDTO : BaseDTO<Guid>
    {
        public FlightLoadDTO(Guid id, DateTime createdAt, DateTime updatedAt,
            string number, DateTime date, int type1SeatNubmer, int type2SeatNubmer, int type3SeatNubmer) : base(id, createdAt, updatedAt)
        {
            Number = number;
            Date = date;
            Type1SeatNumber= type1SeatNubmer;
            Type2SeatNumber= type2SeatNubmer;
            Type3SeatNumber= type3SeatNubmer;
        }

        public string Number { get; set; }
        public DateTime Date { get; set; }
        public int Type1SeatNumber { get; set; }
        public int Type2SeatNumber { get; set; }
        public int Type3SeatNumber { get; set; }
    }
}
