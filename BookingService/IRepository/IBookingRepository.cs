using BookingService.DTO;
using BookingService.Models;

namespace BookingService.IRepository
{
    public interface IBookingRepository
    {
        bool BookTicket(int userId, int showId, int seatId);
        bool CancelTicket(int bookingId);
        List<SeatDetails> GetAvailableSeats(int showId);
        List<Booking> GetUpcomingBooking(int userId);
        List<Booking> GetBookingHistory(int userId);
        List<ShowDetails> GetListedShow(string city);
    }
}
