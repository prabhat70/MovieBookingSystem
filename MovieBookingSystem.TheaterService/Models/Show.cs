namespace MovieBookingSystem.TheaterService.Models
{
    public class Show
    {
        public int ShowId { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        // Navigation properties
        public ICollection<Seat> Seats { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
