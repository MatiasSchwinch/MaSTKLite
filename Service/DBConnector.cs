using MaSTK_Lite.Model;
using Microsoft.EntityFrameworkCore;

namespace MaSTK_Lite.Service
{
    public class DBConnector : DbContext
    {
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DBConnector(DbContextOptions<DBConnector> options) : base(options)
        {
            _ = Database.EnsureCreated();
        }
    }
}
