using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Core.Repositories
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        void ChangeWalletBalance(Wallet wallet, double amount);
    }
}
