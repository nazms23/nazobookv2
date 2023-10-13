using Microsoft.EntityFrameworkCore;

namespace Nazobook.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
            
        }

        
        public DbSet<User> User => Set<User>();
        public DbSet<Gonderi> Gonderi => Set<Gonderi>();
    }
}