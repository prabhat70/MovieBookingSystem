namespace BookingService.Models
{
    public class Seat
    {
        public int SeatId { get; set; }
        public string Row { get; set; }
        public int Number { get; set; }
        public bool IsAvailable { get; set; }

        public int ShowId { get; set; }

        // Navigation properties
        public Show Show { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
