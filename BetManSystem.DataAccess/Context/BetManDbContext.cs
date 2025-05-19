using BetManSystem.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace BetManSystem.DataAccess.Context
{
    public class BetManDbContext : DbContext
    {
        public BetManDbContext(DbContextOptions<BetManDbContext> options)
           : base(options)
        {
        }

        public DbSet<MessageTransmissionLog> MessageTransmissionLogs { get; set; }
        public DbSet<PlayerExternalAccount> PlayerExternalAccounts { get; set; }
        public DbSet<Player> Players { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MessageTransmissionLog>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.PlayerId).IsRequired().HasMaxLength(50);
                entity.Property(e => e.TransactionType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Timestamp).IsRequired();
            });

            builder.Entity<MessageTransmissionLog>(e =>
            {
                e.HasIndex(x => new { x.Provider, x.PlayerId });
                e.HasIndex(x => x.Timestamp).HasDatabaseName("IX_Logs_Timestamp");
            });

            builder.Entity<Player>(e =>
            {
                e.HasIndex(x => x.Email).IsUnique();
                e.HasIndex(x => x.Username).IsUnique();
            });

            builder.Entity<PlayerExternalAccount>(e =>
            {
                e.HasIndex(x => x.PlayerId);
                e.HasIndex(x => new { x.PlayerId, x.Provider, x.ExternalPlayerId })
                 .IsUnique();
            });

            base.OnModelCreating(builder);
        }
    }
}
