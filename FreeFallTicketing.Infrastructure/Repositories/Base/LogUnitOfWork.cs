using SkyDiveTicketing.Core.Repositories.Base;
using SkyDiveTicketing.Infrastructure.Data;

namespace SkyDiveTicketing.Infrastructure.Repositories.Base
{
    public class LogUnitOfWork : ILogUnitOfWork
    {
        private readonly LogContext _context;


        public LogUnitOfWork(LogContext context)
        {
            _context = context;
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
