namespace SkyDiveTicketing.Application.Services.ShoppingCartCheckoutServices
{
    public interface IShoppingCartCheckoutService
    {
        Task<string> Checkout(Guid userId);
        Task<int> Verfiy(Guid userId, string authority);
    }
}
