using SkyDiveTicketing.Application.Base;
using SkyDiveTicketing.Application.Commands.WalletCommands;
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

        public async Task ChargeUserWalletByAdmin(ChargeUserWalletCommand command)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(command.UserId);
            if (user is null)
                throw new ManagedException("کاربر مورد نظر یافت نشد.");

            var wallet = _unitOfWork.WalletRepository.Include(c => c.User).FirstOrDefault(c => c.User == user);
            if (wallet is null)
                throw new ManagedException("کیف پول کاربر یافت نشد.");

            _unitOfWork.WalletRepository.ChangeWalletBalance(wallet, command.Amount);
            
            await _unitOfWork.TransactionRepositroy
                .AddTransaction(string.Empty, string.Empty, "شارژ کیف پول", command.Amount, Core.Entities.TransactionType.Confirmed, user);

            await _unitOfWork.UserRepository
                .AddMessage(user, $"موجودی کیف پول شما {Math.Abs(command.Amount)} ریال {(command.Amount > 0 ? "افزایش" : "کاهش")} پیدا کرد.", "کیف پول");

            await _unitOfWork.CommitAsync();
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
