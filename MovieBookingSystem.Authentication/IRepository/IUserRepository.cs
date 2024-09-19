using MovieBookingSystem.Authentication.DTO;
using MovieBookingSystem.Authentication.Models;

namespace MovieBookingSystem.Authentication.IRepository
{
    public interface IUserRepository
    {
        Task<bool> ValidateUserEmail(UserDetails user);
        Task<string> AddUserDetails(UserDetails user);
        Task<string> GetHashedPassword(string email);
    }
}
