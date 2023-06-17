using Microsoft.EntityFrameworkCore;
using SkyDiveTicketing.Core.Entities.Base;

namespace SkyDiveTicketing.Infrastructure.Data
{
    public class LogContext : DbContext
    {
        public LogContext(DbContextOptions<LogContext> options)
             : base(options)
        {
        }

        public DbSet<ExceptionLog> ExceptionLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
