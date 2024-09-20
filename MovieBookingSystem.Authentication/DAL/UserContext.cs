using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.DAL
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
    }
}
