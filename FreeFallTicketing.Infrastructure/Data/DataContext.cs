using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Entities.Base;
using SkyDiveTicketing.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SkyDiveTicketing.Infrastructure.Data
{
    public class DataContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<DefaultCity> Cities { get; set; }
        public DbSet<FlightLoad> FlightLoads { get; set; }
        public DbSet<FlightLoadCancellationType> FlightLoadCancellationTypes { get; set; }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<SkyDiveEvent> SkyDiveEvents { get; set; }
        public DbSet<SkyDiveEventStatus> SkyDiveEventStatuses { get; set; }
        public DbSet<PassengerDocument> Documents { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<SkyDiveEventTicketType> SkyDiveEventTicketTypes { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyGlobalFilter<BaseEntity>(p => !p.IsDeleted);
        }
    }
}
