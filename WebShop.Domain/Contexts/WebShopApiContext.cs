using Microsoft.EntityFrameworkCore;
using WebShop.Domain.Entities;

namespace WebShop.Domain.Contexts
{
    public sealed class WebShopApiContext : DbContext
    {
        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        
        public WebShopApiContext(DbContextOptions<WebShopApiContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderEntity>()
                .HasMany(x => x.ProductList)
                .WithMany(x => x.OrderList);

            modelBuilder.Entity<OrderEntity>()
                .HasOne(x => x.Client)
                .WithMany(x => x.OrderList)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}