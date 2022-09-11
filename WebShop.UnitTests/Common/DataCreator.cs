using System;
using Microsoft.EntityFrameworkCore;
using WebShop.Domain.Contexts;
using WebShop.Domain.Entities;

namespace WebShop.UnitTests.Common
{
    public class DataCreator
    {
        public static WebShopApiContext Create()
        {
            var options = new DbContextOptionsBuilder<WebShopApiContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new WebShopApiContext(options);
            context.Database.EnsureCreated();

            context.Clients.Add(
                new ClientEntity
                {
                    Id = 1,
                    Email = "admin",
                    Password = "admin",
                    FirstName = "admin",
                    LastName = "admin",
                    PhoneNumber = "admin",
                    IsBaned = false,
                    RoleId = 1
                }
            );
            
            context.Clients.Add(
                new ClientEntity
                {
                    Id = 2,
                    Email = "RegularUser",
                    Password = "RegularUser",
                    FirstName = "RegularUser",
                    LastName = "RegularUser",
                    PhoneNumber = "RegularUser",
                    IsBaned = false,
                    RoleId = 2
                }
            );
            
            context.Clients.Add(
                new ClientEntity
                {
                    Id = 3,
                    Email = "BannedUser",
                    Password = "BannedUser",
                    FirstName = "BannedUser",
                    LastName = "BannedUser",
                    PhoneNumber = "BannedUser",
                    IsBaned = true,
                    RoleId = 2
                }
            );
                
            context.Products.Add(
                new ProductEntity
                {
                    Id = 1,
                    Name = "Product 1",
                    Description = "Description of Product 1",
                    Price = 200
                }
            );

            context.Products.Add(
                new ProductEntity
                {
                    Id = 2,
                    Name = "Product 2",
                    Description = "Description of Product 2",
                    Price = 100
                }
            );

            context.Discounts.Add(
                new DiscountEntity
                {
                    Id = 1,
                    ClientId = 1,
                    ProductId = 2,
                    Discount = 25
                }
            );

            context.Orders.Add(
                new OrderEntity
                {
                    Id = 1,
                    ClientId = 1,
                    TotalPrice = 0
                }
            );

            context.SaveChanges();

            return context;
        }

        public static void Delete(WebShopApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
