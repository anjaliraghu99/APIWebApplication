using Microsoft.EntityFrameworkCore;

namespace APIWebApplication.Models
{
    public class DataDbContext: DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { 
        }
        public DbSet<Products> Products { get; set; }
    }
     
}
