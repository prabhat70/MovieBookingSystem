using AuthenticationPlugin;
using Microsoft.EntityFrameworkCore;
using MovieBookingSystem.Authentication.Authentication;
using MovieBookingSystem.Authentication.DTO;
using MovieBookingSystem.Authentication.IRepository;
using MovieBookingSystem.Authentication.Models;

namespace MovieBookingSystem.Authentication.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _userContext;

        public UserRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<bool> ValidateUserEmail(UserDetails user)
        {
            var email = await _userContext.User
                .Where(u => u.Email == user.Email)
                .Select(u => u.Email)
                .FirstOrDefaultAsync();

            if(email == null)
            {
                return false;
            }
            return true;
        }

        public async Task<string> AddUserDetails(UserDetails user)
        {
            _userContext.User.Add(
                new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = SecurePasswordHasherHelper.Hash(user.Password),
                    Role = user.Role
                });
            await _userContext.SaveChangesAsync();
            return "Details added successfully";
        }
        
        public async Task<string> GetHashedPassword(string email)
        {
            var hashedPassword = await _userContext.User
                .Where(u => u.Email == email)
                .Select(u => u.Password)
                .FirstAsync();
            return hashedPassword;
        }
    }
}
