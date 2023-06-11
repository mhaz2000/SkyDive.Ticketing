using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.DTOs.WalletDTOs;
using SkyDiveTicketing.Core.Repositories.Base;

namespace SkyDiveTicketing.Application.Services.WalletServices
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WalletService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ChargeWallet(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربر مورد نظر یافت نشد.");

            var wallet = await _unitOfWork.WalletRepository.GetFirstWithIncludeAsync(c => c.User == user, c => c.User);

            //TODO

            await _unitOfWork.CommitAsync();
        }

        public async Task<WalletDTO> GetUserWallet(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user is null)
                throw new ManagedException("کاربر مورد نظر یافت نشد.");

            var wallet = await _unitOfWork.WalletRepository.GetFirstWithIncludeAsync(c => c.User == user, c => c.User);

            return new WalletDTO(wallet.Id, wallet.CreatedAt, wallet.UpdatedAt, wallet.User.Id, wallet.Balance);
        }
    }
}
