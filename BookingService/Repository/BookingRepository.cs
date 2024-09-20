using BookingService.DAL;
using BookingService.DTO;
using BookingService.IRepository;
using BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BookingContext _bookingContext;

        public BookingRepository(BookingContext bookingContext)
        {
            _bookingContext = bookingContext;
        }

        public bool BookTicket(int userId, int showId, int seatId)
        {
            if (ReserveSeat(showId, seatId))
            {
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

        public bool CancelTicket(int bookingId)
        {
            var booking = _bookingContext.Bookings
                .FirstOrDefault(b => b.BookingId == bookingId && !b.IsCanceled);
            if (booking != null)
            {
                if (CancelReservation(booking.ShowId, booking.SeatId))
                {
                    booking.IsCanceled = true;
                    _bookingContext.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool ReserveSeat(int showId, int seatId)
        {
            var seat = _bookingContext.Seats
                .FirstOrDefault(s => s.SeatId == seatId && s.ShowId == showId && s.IsAvailable);
            if (seat != null)
            {
                seat.IsAvailable = false;
                _bookingContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool CancelReservation(int showId, int seatId)
        {
            var seat = _bookingContext.Seats
                .FirstOrDefault(s => s.SeatId == seatId && s.ShowId == showId && !s.IsAvailable);
            if (seat != null)
            {
                seat.IsAvailable = true;
                _bookingContext.SaveChanges();
                return true;
            }
            return false;
        }

        public List<SeatDetails> GetAvailableSeats(int showId)
        {
            var availableSeats = _bookingContext.Seats
                .Where(s => s.ShowId == showId && s.IsAvailable)
                .ToList();
            var seatDetails = availableSeats.Select(s => new SeatDetails
            {
                SeatId = s.SeatId,
                Row = s.Row,
                Number = s.Number,
                IsAvailable = s.IsAvailable
            }).ToList();
            return seatDetails;
        }

        public List<Booking> GetUpcomingBooking(int userId)
        {
            var upcomingBookings = _bookingContext.Bookings
                .Where(b => b.UserId == userId && b.Show.StartTime > DateTime.Now && !b.IsCanceled)
                .Include(b => b.Show)
                .Include(b => b.Seat)
                .ToList();
            return upcomingBookings;
        }

        public List<Booking> GetBookingHistory(int userId)
        {
            var bookingHistory=_bookingContext.Bookings
                .Where(b=>b.UserId==userId && (b.Show.StartTime<=DateTime.Now || b.IsCanceled))
                .Include(b => b.Show)
                .Include(b => b.Seat)
                .ToList();
            return bookingHistory;
        }

        public List<ShowDetails> GetListedShow(string city)
        {
            var listedShow = _bookingContext.Shows
                .Where(s => s.City.ToLower().Equals(city.ToLower()) && s.StartTime >= DateTime.Now)
                .ToList();
            var showDetails = listedShow.Select(show => new ShowDetails
            {
                Title = show.Title,
                StartTime = show.StartTime,
                EndTime = show.EndTime,
                TheaterName = show.TheaterName,
                City = show.City
            }).ToList();
            return showDetails;
        }
    }
}
