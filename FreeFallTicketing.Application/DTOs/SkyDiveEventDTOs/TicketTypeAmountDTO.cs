namespace SkyDiveTicketing.Application.DTOs.SkyDiveEventDTOs
{
    public class TicketTypeAmountDTO
    {
        public TicketTypeAmountDTO(double amount, Guid typeId, string type)
        {
            Amount = amount;
            TypeId = typeId;
            Type = type;
        }

        public double Amount { get; set; }
        public Guid TypeId { get; set; }
        public string Type { get; set; }
    }
}
