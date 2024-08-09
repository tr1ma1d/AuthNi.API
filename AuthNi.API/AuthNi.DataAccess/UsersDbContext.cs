using Microsoft.EntityFrameworkCore;



namespace AuthNi.DataAccess
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {
        }


        public DbSet<UserEntity> users { get; set; }
    }
}