﻿using SkyDiveTicketing.Core.Entities;
using SkyDiveTicketing.Core.Entities.Base;
using SkyDiveTicketing.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SkyDiveTicketing.Infrastructure.Data
{
    public class DataContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<FileModel> Files { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<DefaultCity> Cities { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<FlightLoad> FlightLoads { get; set; }
        public DbSet<JumpRecord> JumpRecords { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<SkyDiveEvent> SkyDiveEvents { get; set; }
        public DbSet<AdminCartable> AdminCartables { get; set; }
        public DbSet<UserStatusInfo> UserStatusInfo { get; set; }
        public DbSet<FlightLoadItem> FlightLoadItems { get; set; }
        public DbSet<MedicalDocument> MedicalDocuments { get; set; }
        public DbSet<LogBookDocument> LogBookDocuments { get; set; }
        public DbSet<SkyDiveEventItem> SkyDiveEventItems { get; set; }
        public DbSet<AttorneyDocument> AttorneyDocuments { get; set; }
        public DbSet<ShoppingCartTicket> ShoppingCartTickets { get; set; }
        public DbSet<UserTypeTicketType> UserTypeTicketTypes { get; set; }
        public DbSet<SkyDiveEventStatus> SkyDiveEventStatuses { get; set; }
        public DbSet<NationalCardDocument> NationalCardDocuments { get; set; }
        public DbSet<SkyDiveEventTicketType> SkyDiveEventTicketTypes { get; set; }
        public DbSet<SkyDiveEventTicketTypeAmount> SkyDiveEventTicketTypeAmounts { get; set; }

        //public DbSet<FlightLoadCancellationType> FlightLoadCancellationTypes { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyGlobalFilter<BaseEntity>(p => !p.IsDeleted);

            #region shopping cart and ticket relationship

            modelBuilder.Entity<ShoppingCartTicket>()
                .HasKey(bc => new { bc.ShoppingCartId, bc.TicketId });

            modelBuilder.Entity<ShoppingCartTicket>()
                .HasOne(bc => bc.Ticket)
                .WithMany(b => b.ShoppingCartTickets)
                .HasForeignKey(bc => bc.TicketId);

            modelBuilder.Entity<ShoppingCartTicket>()
                .HasOne(bc => bc.ShoppingCart)
                .WithMany(c => c.ShoppingCartTickets)
                .HasForeignKey(bc => bc.ShoppingCartId);

            #endregion

            #region ticket and user type relationship

            modelBuilder.Entity<UserTypeTicketType>()
                .HasKey(bc => new { bc.TicketTypeId, bc.UserTypeId });

            modelBuilder.Entity<UserTypeTicketType>()
                .HasOne(bc => bc.TicketType)
                .WithMany(b => b.AllowedUserTypes)
                .HasForeignKey(bc => bc.TicketTypeId);

            modelBuilder.Entity<UserTypeTicketType>()
                .HasOne(bc => bc.UserType)
                .WithMany(c => c.AllowedTicketTypes)
                .HasForeignKey(bc => bc.UserTypeId);

            #endregion

            modelBuilder.Entity<Passenger>()
                .HasOne(p => p.MedicalDocumentFile)
                .WithOne()
                .HasForeignKey<Passenger>(p => p.MedicalDocumentFileId);

            modelBuilder.Entity<Passenger>()
                .HasOne(p => p.LogBookDocumentFile)
                .WithOne()
                .HasForeignKey<Passenger>(p => p.LogBookDocumentFileId);

            modelBuilder.Entity<Passenger>()
                .HasOne(p => p.AttorneyDocumentFile)
                .WithOne()
                .HasForeignKey<Passenger>(p => p.AttorneyDocumentFileId);

            modelBuilder.Entity<Passenger>()
                .HasOne(p => p.NationalCardDocumentFile)
                .WithOne()
                .HasForeignKey<Passenger>(p => p.NationalCardDocumentFileId);

            modelBuilder.Entity<Ticket>().HasOne(c => c.ReservedBy).WithMany().HasForeignKey("ReservedById");
        }
    }
}
