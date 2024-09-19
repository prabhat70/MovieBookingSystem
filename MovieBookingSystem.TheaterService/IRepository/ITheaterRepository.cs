using MovieBookingSystem.TheaterService.Models;

namespace MovieBookingSystem.TheaterService.IRepository
{
    public interface ITheaterRepository
    {
        void AllocateSeatInventory(int showId, List<Seat> seats);
        bool ReserveSeat(int showId, int seatId);
        bool CancelReservation(int showId, int seatId);
        List<Seat> GetAvailableSeats(int showId);
        void AddShow(Show show);
        Show GetShowById(int showId);
    }
}
