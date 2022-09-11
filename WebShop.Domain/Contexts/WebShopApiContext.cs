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
        public DbSet<DiscountEntity> Discounts { get; set; } = null!;

        public WebShopApiContext(DbContextOptions<WebShopApiContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Client-Order Many-To-Many

            modelBuilder.Entity<ClientsOrdersEntity>()
                .HasKey(x => new { x.ClientId, x.OrderId });
            
            modelBuilder.Entity<ClientsOrdersEntity>()
                .HasOne(x => x.ClientEntity)
                .WithMany(x => x.ClientsOrdersEntities)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientsOrdersEntity>()
                .HasOne(x => x.OrderEntity)
                .WithMany(x => x.ClientsOrdersEntities)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            

            //Client-Product Many-To-Many
            
            modelBuilder.Entity<ClientsProductsEntity>()
                .HasKey(x => new { x.ClientId, x.ProductId });

            modelBuilder.Entity<ClientsProductsEntity>()
                .HasOne(x => x.ClientEntity)
                .WithMany(x => x.ClientsProductsEntities)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<ClientsProductsEntity>()
                .HasOne(x => x.ProductEntity)
                .WithMany(x => x.ClientsProductsEntities)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            
            //Client-Discount Many-To-Many

            modelBuilder.Entity<ClientsDiscountEntity>()
                .HasKey(x => new { x.ClientId, x.DiscountId });

            modelBuilder.Entity<ClientsDiscountEntity>()
                .HasOne(x => x.ClientEntity)
                .WithMany(x => x.ClientsDiscountEntities)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientsDiscountEntity>()
                .HasOne(x => x.DiscountEntity)
                .WithMany(x => x.ClientsDiscountEntities)
                .HasForeignKey(x => x.DiscountId)
                .OnDelete(DeleteBehavior.Cascade);
            
            
            //Basic role data
            
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
            
            modelBuilder.Entity<RoleEntity>().HasData( adminRole, userRole );
        }
    }
}