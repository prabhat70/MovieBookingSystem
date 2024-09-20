using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TheaterService.DAL;
using TheaterService.DTO;
using TheaterService.IRepository;
using TheaterService.Models;

namespace TheaterService.Repository
{
    public class TheaterRepository : ITheaterRepository
    {
        private readonly TheaterContext _theaterServiceContext;

        public TheaterRepository(TheaterContext theaterServiceContext)
        {
            _theaterServiceContext = theaterServiceContext;
        }
        
        public void AllocateSeatInventory(int showId, List<SeatDetails> seats)
        {
            var show = _theaterServiceContext.Shows.Find(showId);
            if (show != null)
            {
                foreach (var seat in seats)
                {
                    show.Seats.Add(
                        new Seat
                        {
                            Row= seat.Row,
                            Number = seat.Number,
                            IsAvailable = seat.IsAvailable,
                            ShowId = showId
                        });
                }
                _theaterServiceContext.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"Show with ID {showId} not found.");
            }
        }

        public bool ReserveSeat(int showId, int seatId)
        {
            var seat = _theaterServiceContext.Seats
                .FirstOrDefault(s => s.SeatId == seatId && s.ShowId == showId && s.IsAvailable);
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

        public List<SeatDetails> GetAvailableSeats(int showId)
        {
            var availableSeats = _theaterServiceContext.Seats.Where(s => s.ShowId == showId && s.IsAvailable).ToList();
            var seatDetails = availableSeats.Select(s => new SeatDetails
            {
                SeatId = s.SeatId,
                Row = s.Row,
                Number = s.Number,
                IsAvailable = s.IsAvailable
            }).ToList();
            return seatDetails;
        }

        public void AddShow(ShowDetails showDetails)
        {
            _theaterServiceContext.Shows.Add(
                new Show
                {
                    Title = showDetails.Title,
                    StartTime = showDetails.StartTime,
                    EndTime = showDetails.EndTime,
                    City = showDetails.City,
                    TheaterName = showDetails.TheaterName
                });
            _theaterServiceContext.SaveChanges();
        }

        public Show GetShowById(int showId)
        {
            return _theaterServiceContext.Shows
                .Where(s => s.ShowId == showId)
                .Include(s => s.Seats)
                .Include(s => s.Bookings)
                .First();
        }
    }
}
