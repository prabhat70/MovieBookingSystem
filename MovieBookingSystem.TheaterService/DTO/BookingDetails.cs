namespace TheaterService.DTO
{
    public class BookingDetails
    {
        public int BookingId { get; set; }
        public DateTime BookingTime { get; set; }
        public bool IsCanceled { get; set; }

        public int UserId { get; set; }
        public int ShowId { get; set; }
        public int SeatId { get; set; }
    }
}
