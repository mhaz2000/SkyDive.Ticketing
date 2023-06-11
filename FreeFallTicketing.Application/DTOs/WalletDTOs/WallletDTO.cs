namespace SkyDiveTicketing.Application.DTOs.WalletDTOs
{
    public class WalletDTO : BaseDTO<Guid>
    {
        public WalletDTO(Guid id, DateTime createdAt, DateTime updatedAt, Guid userId, double balance) : base(id, createdAt, updatedAt)
        {
            UserId = userId;
            Balance = balance;
        }

        public Guid UserId { get; set; }
        public double Balance { get; set; }
    }
}
