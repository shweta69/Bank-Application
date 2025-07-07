using Bank_App_DB_Context_Repo.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bank_App_DB_Context_Repo
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } 
    }
}
