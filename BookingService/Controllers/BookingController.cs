using BookingService.DTO;
using BookingService.IRepository;
using BookingService.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers
{
    [Authorize(Roles = "Customer")]
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        [HttpPost("book-ticket")]
        public IActionResult BookTicket([FromQuery] int userId, [FromQuery] int showId, [FromQuery] int seatId)
        {
            var success = _bookingRepository.BookTicket(userId, showId, seatId);
            if (success)
            {
                return Ok("Ticket booked successfully.");
            }
            return BadRequest("Failed to book ticket. Selected seat may not be available.");
        }

        [HttpPost("cancel-ticket")]
        public IActionResult CancelTicket([FromQuery] int bookingId)
        {
            var success = _bookingRepository.CancelTicket(bookingId);
            if (success)
            {
                return Ok("Ticket canceled successfully.");
            }
            return BadRequest("Failed to cancel Ticket.");
        }

        [HttpGet("available-seats/{showId}")]
        public IActionResult CheckSeatAvailability(int showId)
        {
            var seats = _bookingRepository.GetAvailableSeats(showId);
            return Ok(seats);
        }

        [HttpGet("upcoming-bookings/{userId}")]
        public IActionResult GetUpcomingBookings(int userId)
        {
            var bookings = _bookingRepository.GetUpcomingBooking(userId);

            if(bookings == null || !bookings.Any())
            {
                return NotFound("No upcoming bookings found.");
            }

            var bookingDetails = bookings.Select(b => new BookingDetails
            {
                BookingId = b.BookingId,
                BookingTime = b.BookingTime,
                IsCanceled = b.IsCanceled,
                UserId = userId,
                ShowId = b.ShowId,
                SeatId = b.SeatId,
                Show = new ShowDetails
                {
                    Title = b.Show.Title,
                    StartTime = b.Show.StartTime,
                    EndTime = b.Show.EndTime,
                    City = b.Show.City,
                    TheaterName = b.Show.TheaterName
                },
                Seat = new SeatDetails
                {
                    SeatId = b.Seat.SeatId,
                    Row = b.Seat.Row,
                    Number = b.Seat.Number,
                    IsAvailable = b.Seat.IsAvailable
                }
            });

            return Ok(bookingDetails);
        }

        [HttpGet("booking-history/{userId}")]
        public IActionResult GetBookingHistory(int userId)
        {
            var bookings = _bookingRepository.GetBookingHistory(userId);

            if (bookings == null || !bookings.Any())
            {
                return NotFound("No booking history found.");
            }

            var bookingDetails = bookings.Select(b => new BookingDetails
            {
                BookingId = b.BookingId,
                BookingTime = b.BookingTime,
                IsCanceled = b.IsCanceled,
                UserId = userId,
                ShowId = b.ShowId,
                SeatId = b.SeatId,
                Show = new ShowDetails
                {
                    Title = b.Show.Title,
                    StartTime = b.Show.StartTime,
                    EndTime = b.Show.EndTime,
                    City = b.Show.City,
                    TheaterName = b.Show.TheaterName
                },
                Seat = new SeatDetails
                {
                    SeatId = b.Seat.SeatId,
                    Row = b.Seat.Row,
                    Number = b.Seat.Number,
                    IsAvailable = b.Seat.IsAvailable
                }
            });

            return Ok(bookingDetails);
        }

        [HttpGet("listed-movies")]
        public IActionResult GetListedShow([FromQuery] string city)
        {
            var shows = _bookingRepository.GetListedShow(city);
            if(shows == null || !shows.Any())
            {
                return NotFound("No movie listed in this city at this moment");
            }
            return Ok(shows);
        }
    }
}
