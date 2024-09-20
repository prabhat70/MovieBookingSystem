namespace BookingService.DTO
{
    public class SeatAvailability
    {
        public int SeatId { get; set; }
        public string Row { get; set; }
        public int Number { get; set; }
        public bool IsAvailable { get; set; }

        public int ShowId { get; set; }
    }
}
