using BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.DAL
{
    public class BookingContext : DbContext
    {
        public BookingContext(DbContextOptions<BookingContext> options) : base(options)
        {
            
        }

        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
