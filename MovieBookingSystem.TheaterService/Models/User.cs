namespace MovieBookingSystem.TheaterService.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // Navigation properties
        public ICollection<Booking> Bookings { get; set; }
    }
}
