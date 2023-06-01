namespace SkyDiveTicketing.Application.Helpers
{
    public interface ITokenFactory
    {
        string GenerateToken(int size = 32);
    }
}
