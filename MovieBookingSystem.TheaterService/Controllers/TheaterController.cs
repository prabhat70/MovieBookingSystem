using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBookingSystem.TheaterService.IRepository;
using MovieBookingSystem.TheaterService.Models;

namespace MovieBookingSystem.TheaterService.Controllers
{
    [Authorize(Roles = "string")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TheaterController : ControllerBase
    {
        private readonly ITheaterRepository _theaterRepository;

        public TheaterController(ITheaterRepository theaterRepository)
        {
            _theaterRepository = theaterRepository;
        }

        // Endpoint to add a new show
        [HttpPost("add-show")]
        public IActionResult AddShow([FromBody] Show show)
        {
            if (show == null)
            {
                return BadRequest("Show data is null.");
            }

            try
            {
                _theaterRepository.AddShow(show);
                return CreatedAtAction(nameof(GetShow), new { id = show.ShowId }, show);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Endpoint to get a show by ID
        [HttpGet("show/{id}")]
        public IActionResult GetShow(int id)
        {
            var show = _theaterRepository.GetShowById(id); // Assume this method exists in ITheaterService
            if (show == null)
            {
                return NotFound();
            }
            return Ok(show);
        }

        // Endpoint to get available seats for a show
        [HttpGet("available-seats/{showId}")]
        public IActionResult GetAvailableSeats(int showId)
        {
            var seats = _theaterRepository.GetAvailableSeats(showId);
            return Ok(seats);
        }

        // Endpoint to reserve a seat for a show
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

        // Endpoint to cancel a reservation
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
