namespace TheaterService.Models
{
    public class Show
    {
        public int ShowId { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string City { get; set; }
        public string TheaterName { get; set; }
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
