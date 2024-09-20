namespace TheaterService.DTO
{
    public class ShowResponse
    {
        public int ShowId { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string City { get; set; }
        public string TheaterName { get; set; }
        public ICollection<SeatDetails> Seats { get; set; }
        public ICollection<BookingDetails> Bookings { get; set; }
    }
}
