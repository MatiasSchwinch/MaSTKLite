using MaSTK_Lite.Model;
using Microsoft.EntityFrameworkCore;

namespace MaSTK_Lite.Service
{
    public class DBConnector : DbContext
    {
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DBConnector(DbContextOptions<DBConnector> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region BD MaSTK
            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.ToTable("Warehouse");

                entity.HasKey(k => k.WarehouseID);

                entity.Property(p => p.Name).HasColumnType("nvarchar(100)");

                entity.Property(p => p.Description).HasColumnType("nvarchar(240)");

                entity.HasMany(r => r.Products)
                      .WithOne(r => r.Warehouse)
                      .HasForeignKey(fk => fk.WarehouseID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.HasKey(k => k.ProductID);

                entity.Property(p => p.Date).HasColumnType("date");

                entity.Property(p => p.ProductSKU).HasColumnType("nvarchar(20)");

                entity.Property(p => p.Brand).HasColumnType("nvarchar(50)");

                entity.Property(p => p.Model).HasColumnType("nvarchar(40)");

                entity.Property(p => p.Description).HasColumnType("nvarchar(240)");

                entity.Property(p => p.Stock).HasColumnType("int");

                entity.Property(p => p.Price).HasColumnType("money");

                entity.HasOne(r => r.Category)
                      .WithMany(r => r.Product)
                      .HasForeignKey(fk => fk.CategoryID)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.HasKey(k => k.CategoryID);

                entity.Property(p => p.Name).HasColumnType("nvarchar(60)");
            });
            #endregion
        }
    }
}
