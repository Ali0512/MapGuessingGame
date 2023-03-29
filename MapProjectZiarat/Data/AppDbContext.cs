using MapProjectZiarat.Models;
using Microsoft.EntityFrameworkCore;

namespace MapProjectZiarat.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Map> maps { get; set; }
    }
}
