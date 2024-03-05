namespace SkyDiveTicketing.Application.DTOs.ShoppingCartDTOs
{
    public class PaidShoppingCartDTO : ShoppingCartDTO
    {
        public PaidShoppingCartDTO(string refId) : base(Guid.Empty, DateTime.Now, DateTime.Now)
        {
            RefId = refId;
        }

        public string RefId { get; set; }
    }
}
