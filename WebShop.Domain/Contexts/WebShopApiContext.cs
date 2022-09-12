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
        public DbSet<ClientsProductsEntity> ClientsProducts { get; set; } = null!;
        public DbSet<ClientsDiscountsEntity> ClientsDiscounts { get; set; } = null!;

        public WebShopApiContext(DbContextOptions<WebShopApiContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Client-Order One-To-Many

            modelBuilder.Entity<OrderEntity>()
                .HasOne(x => x.Client)
                .WithMany(x => x.Orders)
                .OnDelete(DeleteBehavior.Cascade);
            
            //Product-Discount One-To-Many

            modelBuilder.Entity<DiscountEntity>()
                .HasOne(x => x.Product)
                .WithMany(x => x.Discounts)
                .OnDelete(DeleteBehavior.Cascade);
                
                
            //Client-Product Many-To-Many

            modelBuilder.Entity<ClientEntity>()
                .HasMany(x => x.ClientsProductsEntities)
                .WithOne(x => x.ClientEntity)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<ProductEntity>()
                .HasMany(x => x.ClientsProductsEntities)
                .WithOne(x => x.ProductEntity)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            
            //Client-Discount Many-To-Many

            modelBuilder.Entity<ClientEntity>()
                .HasMany(x => x.ClientsDiscountsEntities)
                .WithOne(x => x.ClientEntity)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DiscountEntity>()
                .HasMany(x => x.ClientsDiscountsEntities)
                .WithOne(x => x.DiscountEntity)
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