using Microsoft.EntityFrameworkCore;

namespace Market.Models
{
    public class MarketContext: DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<ProductStore> ProductStores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.; Database=Store;TrustServerCertificate=True; Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(x => x.Id).HasName("ProductId");
                entity.Property(e => e.Name).HasColumnName("ProductName").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Description).HasColumnName("Description").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Cost).HasColumnName("Cost").IsRequired();
                entity.HasOne(x => x.Category).WithMany(c => c.Products).HasForeignKey(x => x.Id).HasConstraintName("CategoryToProduct");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(x => x.Id).HasName("CategoryId");
                entity.HasIndex(x => x.Name).IsUnique();
                entity.Property(e => e.Name).HasColumnName("CategoryName").HasMaxLength(255).IsRequired();
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Stores");
                entity.HasKey(x => x.Id).HasName("StorageId");
                entity.Property(e => e.Name).HasColumnName("StoreName");
            });

            modelBuilder.Entity<ProductStore>().HasKey(sc => new { sc.ProductId, sc.StoreId });

        }

    }
}
