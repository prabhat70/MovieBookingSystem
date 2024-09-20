using UserService.DTO;
using UserService.Models;

namespace UserService.IRepository
{
    public interface IUserRepository
    {
        Task<bool> ValidateUserEmail(string Email);
        Task<string> AddUserDetails(UserRegistration user);
        Task<User> GetUserDetails(string email);
        Task<bool> DeactivateAccount(string email);
    }
}
