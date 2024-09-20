using AuthenticationPlugin;
using Microsoft.EntityFrameworkCore;
using UserService.DAL;
using UserService.DTO;
using UserService.IRepository;
using UserService.Models;

namespace UserService.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _userContext;

        public UserRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<bool> ValidateUserEmail(string Email)
        {
            var email = await _userContext.User
                .Where(u => u.Email == Email)
                .Select(u => u.Email)
                .FirstOrDefaultAsync();

            if(email == null)
            {
                return false;
            }
            return true;
        }

        public async Task<string> AddUserDetails(UserRegistration user)
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

        public async Task<User> GetUserDetails(string email)
        {
            var userDetails = await _userContext.User
                .Where(u => u.Email == email)
                .FirstAsync();
            return userDetails;
        }

        public async Task<bool> DeactivateAccount(string email)
        {
            var response = await _userContext.User
                .Where(u => u.Email == email)
                .ExecuteDeleteAsync();
            if (response > 0)
            {
                return true;
            }
            return false;
        }
    }
}
