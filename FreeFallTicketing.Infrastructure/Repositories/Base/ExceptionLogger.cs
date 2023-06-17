using SkyDiveTicketing.Core.Entities.Base;
using SkyDiveTicketing.Core.Repositories.Base;
using SkyDiveTicketing.Infrastructure.Data;

namespace SkyDiveTicketing.Infrastructure.Repositories.Base
{
    public class ExceptionLogger : IExceptionLogger
    {
        private readonly LogContext _context;

        public ExceptionLogger(LogContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task LogAsync(ExceptionLog log)
        {
            _context.ExceptionLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
