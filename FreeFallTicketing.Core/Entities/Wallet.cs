using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Entities
{
    public class Wallet : BaseEntity
    {
        public Wallet()
        {

        }

        public Wallet(double balance, User user) : base()
        {
            Balance = balance;
            User = user;
        }

        public double Balance { get; private set; }
        public User User { get; set; }

        public void ChangeBalance(double amount) => Balance += amount;
    }
}
