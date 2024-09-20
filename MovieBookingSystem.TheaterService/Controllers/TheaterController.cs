using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheaterService.DTO;
using TheaterService.IRepository;

namespace TheaterService.Controllers
{
    [Authorize(Roles = "TheaterOwner")]
    [ApiController]
    [Route("api/[controller]")]
    public class TheaterController : ControllerBase
    {
        private readonly ITheaterRepository _theaterRepository;

        public TheaterController(ITheaterRepository theaterRepository)
        {
            _theaterRepository = theaterRepository;
        }

        [HttpPost("add-show")]
        public IActionResult AddShow([FromBody] ShowDetails showDetails)
        {
            if (showDetails == null)
            {
                return BadRequest("Show data is null.");
            }

            try
            {
                _theaterRepository.AddShow(showDetails);
                return Ok("Show added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("{showId}/allocate-seats")]
        public IActionResult AllocateSeatInventory(int showId, [FromBody] List<SeatDetails> seats)
        {
            if (seats == null)
            {
                return BadRequest("Seat list cannot be null or empty.");
            }

            try
            {
                _theaterRepository.AllocateSeatInventory(showId, seats);
                return Ok($"Seat successfully allocated for show ID {showId}.");
            }
            catch(ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("show/{id}")]
        public IActionResult GetShow(int id)
        {
            var show = _theaterRepository.GetShowById(id);
            if (show == null)
            {
                return NotFound();
            }
            var showResponse = new ShowResponse
            {
                ShowId = show.ShowId,
                Title = show.Title,
                StartTime = show.StartTime,
                EndTime = show.EndTime,
                City = show.City,
                TheaterName = show.TheaterName,
                Seats = show.Seats.Select(s => new SeatDetails
                {
                    SeatId = s.SeatId,
                    Row = s.Row,
                    Number = s.Number,
                    IsAvailable = s.IsAvailable
                }).ToList(),
                Bookings = show.Bookings.Select(b => new BookingDetails
                {
                    BookingId = b.BookingId,
                    BookingTime = b.BookingTime,
                    IsCanceled = b.IsCanceled
                }).ToList()
            };
            return Ok(showResponse);
        }

        [HttpGet("available-seats/{showId}")]
        public IActionResult GetAvailableSeats(int showId)
        {
            var seats = _theaterRepository.GetAvailableSeats(showId);
            return Ok(seats);
        }

        [HttpPost("reserve-seat")]
        public IActionResult ReserveSeat([FromQuery] int showId, [FromQuery] int seatId)
        {
            var success = _theaterRepository.ReserveSeat(showId, seatId);
            if (success)
            {
                return Ok("Seat reserved successfully.");
            }
            return BadRequest("Failed to reserve seat. It may not be available.");
        }

        [HttpPost("cancel-reservation")]
        public IActionResult CancelReservation([FromQuery] int showId, [FromQuery] int seatId)
        {
            var success = _theaterRepository.CancelReservation(showId, seatId);
            if (success)
            {
                return Ok("Reservation canceled successfully.");
            }
            return BadRequest("Failed to cancel reservation.");
        }
    }
}
