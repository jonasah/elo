using Elo.Models;
using Microsoft.EntityFrameworkCore;

namespace Elo.DbHandler
{
    public class EloDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameScore> GameScores { get; set; }
        public DbSet<PlayerRating> Ratings { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<PlayerSeason> PlayerSeasons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EloDb;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // player name is unique
            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Name)
                .IsUnique();

            // season name is unique
            modelBuilder.Entity<Season>()
                .HasIndex(s => s.Name)
                .IsUnique();

            // delete player ratings on game deletion
            modelBuilder.Entity<PlayerRating>()
                .HasOne(pr => pr.Game)
                .WithMany(g => g.PlayerRatings)
                .OnDelete(DeleteBehavior.Cascade);

            // set default values for Created properties
            modelBuilder.Entity<Player>()
                .Property(p => p.Created)
                .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<Game>()
                .Property(p => p.Created)
                .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<GameScore>()
                .Property(p => p.Created)
                .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<PlayerRating>()
                .Property(p => p.Created)
                .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<Season>()
                .Property(p => p.Created)
                .HasDefaultValueSql("getutcdate()");

            modelBuilder.Entity<PlayerSeason>()
                .Property(p => p.Created)
                .HasDefaultValueSql("getutcdate()");
        }
    }
}
