namespace SkyDiveTicketing.Application.DTOs.FlightLoadDTOs
{
    public class FlightLoadCancellationTypeDTO : BaseDTO<Guid>
    {
        public FlightLoadCancellationTypeDTO(Guid id, DateTime createdAt, DateTime updatedAt, Guid flightLoadId, int hoursBeforeCancellation, float rate)
            : base(id, createdAt, updatedAt)
        {
            FlightLoadId = flightLoadId;
            HoursBeforeCancellation = hoursBeforeCancellation;
            Rate = rate;
        }

        public Guid FlightLoadId { get; set; }
        public int HoursBeforeCancellation { get; set; }
        public float Rate { get; set; }
    }
}
