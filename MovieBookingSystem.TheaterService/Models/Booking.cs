namespace MovieBookingSystem.TheaterService.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateTime BookingTime { get; set; }
        public bool IsCanceled { get; set; }

        public int UserId { get; set; }
        public int ShowId { get; set; }
        public int SeatId { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Show Show { get; set; }
        public Seat Seat { get; set; }
    }
}
