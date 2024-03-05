using SkyDiveTicketing.Application.DTOs.ShoppingCartDTOs;

namespace SkyDiveTicketing.Application.Services.ShoppingCartCheckoutServices
{
    public interface IShoppingCartCheckoutService
    {
        Task<string> Checkout(Guid userId);
        Task<PaidShoppingCartDTO> Verfiy(Guid userId, string authority);
    }
}
