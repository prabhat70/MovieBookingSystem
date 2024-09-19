using Microsoft.EntityFrameworkCore;
using MovieBookingSystem.Authentication.Models;

namespace MovieBookingSystem.Authentication.Authentication
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
    }
}
