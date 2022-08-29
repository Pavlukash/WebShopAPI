using Microsoft.EntityFrameworkCore;
using WebShop.Domain.Entities;

namespace WebShop.Domain.Contexts
{
    public sealed class WebShopApiContext : DbContext
    {
        public DbSet<ClientEntity> Clients { get; set; } = null!;
        public DbSet<OrderEntity> Orders { get; set; } = null!;
        public DbSet<ProductEntity> Products { get; set; } = null!;

        public DbSet<RoleEntity> Roles { get; set; } = null!;

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
                
            string adminRoleName = "admin";
            string userRoleName = "user";
            
            RoleEntity adminRole = new RoleEntity 
                { 
                    Id = 1, 
                    Name = adminRoleName
                };
            
            RoleEntity userRole = new RoleEntity
            {
                Id = 2, 
                Name = userRoleName
            };
            
            modelBuilder.Entity<RoleEntity>().HasData(new RoleEntity[] { adminRole, userRole });
        }
    }
}