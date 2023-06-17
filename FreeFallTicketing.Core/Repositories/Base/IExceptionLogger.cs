using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Core.Repositories.Base
{
    public interface IExceptionLogger
    {
        Task LogAsync(ExceptionLog log);
    }
}
