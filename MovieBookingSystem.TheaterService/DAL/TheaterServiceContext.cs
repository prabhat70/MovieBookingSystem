using Microsoft.EntityFrameworkCore;
using MovieBookingSystem.TheaterService.Models;

namespace MovieBookingSystem.TheaterService.DAL
{
    public class TheaterServiceContext : DbContext
    {
        public TheaterServiceContext(DbContextOptions<TheaterServiceContext> options) : base(options)
        {
            
        }

        // Define DbSet properties for each table
        public DbSet<User> Users { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define entity configurations (optional)
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Show>(entity =>
            {
                entity.ToTable("Show");
                entity.HasKey(e => e.ShowId);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.StartTime).IsRequired();
                entity.Property(e => e.EndTime).IsRequired();
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.ToTable("Seat");
                entity.HasKey(e => e.SeatId);
                entity.Property(e => e.Row).IsRequired().HasMaxLength(5);
                entity.Property(e => e.Number).IsRequired();
                entity.Property(e => e.IsAvailable).IsRequired();

                // Define relationship with Show
                entity.HasOne(s => s.Show)
                      .WithMany(s => s.Seats)
                      .HasForeignKey(s => s.ShowId);
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");
                entity.HasKey(e => e.BookingId);
                entity.Property(e => e.BookingTime).IsRequired();
                entity.Property(e => e.IsCanceled).IsRequired();

                // Define relationships
                entity.HasOne(b => b.User)
                      .WithMany(u => u.Bookings)
                      .HasForeignKey(b => b.UserId);

                entity.HasOne(b => b.Show)
                      .WithMany(s => s.Bookings)
                      .HasForeignKey(b => b.ShowId);

                entity.HasOne(b => b.Seat)
                      .WithMany(s => s.Bookings)
                      .HasForeignKey(b => b.SeatId);
            });
        }
    }
}
