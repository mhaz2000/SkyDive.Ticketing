namespace SkyDiveTicketing.Application.PaymentServices
{
    public interface IPaymentService
    {
        Task<string> Checkout(double amount);
        Task<ulong> Verify(string authority, double amount);
    }

    public interface IZarinpalPaymentService : IPaymentService { }
}
