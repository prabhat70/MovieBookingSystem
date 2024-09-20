using TheaterService.DTO;
using TheaterService.Models;

namespace TheaterService.IRepository
{
    public interface ITheaterRepository
    {
        void AllocateSeatInventory(int showId, List<SeatDetails> seats);
        bool ReserveSeat(int showId, int seatId);
        bool CancelReservation(int showId, int seatId);
        List<SeatDetails> GetAvailableSeats(int showId);
        void AddShow(ShowDetails showDetails);
        Show GetShowById(int showId);
    }
}
