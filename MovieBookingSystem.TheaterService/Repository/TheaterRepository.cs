using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MovieBookingSystem.TheaterService.DAL;
using MovieBookingSystem.TheaterService.IRepository;
using MovieBookingSystem.TheaterService.Models;

namespace MovieBookingSystem.TheaterService.Repository
{
    public class TheaterRepository : ITheaterRepository
    {
        private readonly TheaterServiceContext _theaterServiceContext;

        public TheaterRepository(TheaterServiceContext theaterServiceContext)
        {
            _theaterServiceContext = theaterServiceContext;
        }
        // Allocate seat inventory for a show
        public void AllocateSeatInventory(int showId, List<Seat> seats)
        {
            var show = _theaterServiceContext.Shows.Find(showId);
            if (show != null)
            {
                foreach (var seat in seats)
                {
                    show.Seats.Add(seat);  // Assuming Seats has an Add method
                }
                _theaterServiceContext.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"Show with ID {showId} not found.");
            }
        }

        // Reserve a seat for a specific show
        public bool ReserveSeat(int showId, int seatId)
        {
            var seat = _theaterServiceContext.Seats.FirstOrDefault(s => s.SeatId == seatId && s.ShowId == showId && s.IsAvailable);
            if (seat != null)
            {
                seat.IsAvailable = false;
                _theaterServiceContext.SaveChanges();
                return true;
            }
            return false;
        }

        // Cancel a reservation and make the seat available again
        public bool CancelReservation(int showId, int seatId)
        {
            var seat = _theaterServiceContext.Seats.FirstOrDefault(s => s.SeatId == seatId && s.ShowId == showId && !s.IsAvailable);
            if (seat != null)
            {
                seat.IsAvailable = true;
                _theaterServiceContext.SaveChanges();
                return true;
            }
            return false;
        }

        // Get all available seats for a show
        public List<Seat> GetAvailableSeats(int showId)
        {
            return _theaterServiceContext.Seats.Where(s => s.ShowId == showId && s.IsAvailable).ToList();
        }

        // Method to add a new show
        public void AddShow(Show show)
        {
            // Add the show to the Shows DbSet and save the changes to the database
            _theaterServiceContext.Shows.Add(show);
            _theaterServiceContext.SaveChanges();
        }

        public Show GetShowById(int showId)
        {
            return _theaterServiceContext.Shows.Where(s => s.ShowId == showId).First();
        }
    }
}
