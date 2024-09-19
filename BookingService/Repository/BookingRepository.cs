using BookingService.DAL;
using BookingService.Models;
using MovieBookingSystem.TheaterService.IRepository;
using MovieBookingSystem.TheaterService.Repository;

namespace BookingService.Repository
{
    public class BookingRepository
    {
        private readonly BookingContext _bookingContext;
        private readonly ITheaterRepository _theaterService;

        public BookingRepository(BookingContext bookingContext, ITheaterRepository theaterService)
        {
            _bookingContext = bookingContext;
            _theaterService = theaterService;
        }

        // Book a ticket for a user
        public bool BookTicket(int userId, int showId, int seatId)
        {
            // Check if the seat is available via Theater Service
            if (_theaterService.ReserveSeat(showId, seatId))
            {
                // Create a booking record
                var booking = new Booking
                {
                    UserId = userId,
                    ShowId = showId,
                    SeatId = seatId,
                    BookingTime = DateTime.UtcNow
                };
                _bookingContext.Bookings.Add(booking);
                _bookingContext.SaveChanges();
                return true;
            }
            return false;
        }

        // Cancel a ticket by bookingId
        public bool CancelTicket(int bookingId)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId && !b.IsCanceled);
            if (booking != null)
            {
                if (_theaterService.CancelReservation(booking.ShowId, booking.SeatId))
                {
                    booking.IsCanceled = true;
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        // Check seat availability for a particular show
        public List<SeatAvailability> CheckSeatAvailability(int showId)
        {
            var availableSeats = _theaterService.GetAvailableSeats(showId);
            return availableSeats.Select(s => new SeatAvailability
            {
                ShowId = showId,
                SeatId = s.SeatId,
                IsAvailable = s.IsAvailable
            }).ToList();
        }
    }
}
