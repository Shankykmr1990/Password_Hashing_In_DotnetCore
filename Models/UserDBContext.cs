using Microsoft.EntityFrameworkCore;

namespace PasswordHashingDemo.Models
{
    public class UserDBContext : DbContext
    {
        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
