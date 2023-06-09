﻿using SkyDiveTicketing.Application.Commands.WalletCommands;
using SkyDiveTicketing.Application.DTOs.WalletDTOs;

namespace SkyDiveTicketing.Application.Services.WalletServices
{
    public interface IWalletService
    {
        Task<WalletDTO> GetUserWallet(Guid userId);

        Task ChargeWallet(Guid userId);

        Task ChargeUserWalletByAdmin(ChargeUserWalletCommand command);
    }
}
