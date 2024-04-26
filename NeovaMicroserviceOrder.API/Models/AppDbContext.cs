using Microsoft.EntityFrameworkCore;

namespace NeovaMicroserviceOrder.API.Models
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
    }
}