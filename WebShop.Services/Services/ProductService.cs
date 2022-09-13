using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShop.Domain.Contexts;
using WebShop.Domain.Entities;
using WebShop.Domain.Models;
using WebShop.Services.Extentions;
using WebShop.Services.Interfaces;
using WebShop.Services.Mappers;

namespace WebShop.Services.Services
{
    public class ProductService : IProductService
    {
        private WebShopApiContext WebShopApiContext { get; }
        private ICurrentUserService CurrentUserService { get; }

        public ProductService(WebShopApiContext context, ICurrentUserService currentUserService)
        {
            WebShopApiContext = context;
            CurrentUserService = currentUserService;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts(CancellationToken cancellationToken)
        {
            var products = await WebShopApiContext.Products
                .AsNoTracking()
                .Select(x => x.ToDto())
                .ToListAsync(cancellationToken);

            return products;
        }
        
        public async Task<ProductDto> GetById(int id, CancellationToken cancellationToken)
        {
            var product = await WebShopApiContext.Products
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrNotFoundAsync(cancellationToken);
            
            var result = product.ToDto();
            
            var discount = await WebShopApiContext.ClientsDiscounts
                .AsNoTracking()
                .Include(x => x.DiscountEntity)
                .Where(x => x.ClientId == 1)
                .Where(x => x.DiscountEntity.ProductId == product.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (discount != null)
            {
                result.NewPrice = product.Price * (1 - discount.DiscountEntity.Discount / 100);
            }

            return result;
        }

        public async Task<List<ProductDto>> GetClientsProductList(CancellationToken cancellationToken)
        {
            var clientsProducts = await WebShopApiContext.ClientsProducts
                .AsNoTracking()
                .Include(x => x.ProductEntity)
                .Where(x => x.ClientId == 1)
                .Select(x => x.ProductEntity.ToDto())
                .ToListAsync(cancellationToken);

            return clientsProducts;
        }
            

        public async Task<bool> AddToProductList(int id, CancellationToken cancellationToken)
        {
            var product = await WebShopApiContext.Products
                .Where(x => x.Id == id)
                .FirstOrNotFoundAsync(cancellationToken);

            var client = await CurrentUserService.GetCurrentUser(cancellationToken);

            var newEntity = new ClientsProductsEntity
            {
                ClientId = client.Id,
                ProductId = product.Id,
            };

            WebShopApiContext.ClientsProducts.Add(newEntity);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> RemoveFromProductList(int id, CancellationToken cancellationToken)
        {
            var clientsProduct = await WebShopApiContext.ClientsProducts
                .Where(x => x.ClientId == 1)
                .Where(x => x.ProductId == id)
                .FirstOrNotFoundAsync(cancellationToken);
            
            WebShopApiContext.ClientsProducts.Remove(clientsProduct);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<ProductDto> Create(ProductDto newProductEntity, CancellationToken cancellationToken)
        {
            ValidateCreateRequest(newProductEntity);

            bool isAdmin = await CurrentUserService.CheckAdmin(null, cancellationToken);
            
            if (isAdmin == false)
            {
                throw new UnauthorizedAccessException("You are not an admin");
            }
            
            var newEntity = new ProductEntity()
            {
                Name = newProductEntity.Name!,
                Description = newProductEntity.Description!,
                Price = newProductEntity.Price
            };

            WebShopApiContext.Products.Add(newEntity);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            var result = newEntity.ToDto();

            return result;
        }

        public async Task<bool> Update(int id, ProductDto productEntity, CancellationToken cancellationToken)
        {
            bool isAdmin = await CurrentUserService.CheckAdmin(null, cancellationToken);
            
            if (isAdmin == false)
            {
                throw new UnauthorizedAccessException("You are not an admin");
            }
            
            var productToUpdate = await WebShopApiContext.Products
                .Where(x => x.Id == id)
                .FirstOrNotFoundAsync(cancellationToken);

            productToUpdate.Name = productEntity.Name ?? productToUpdate.Name;
            productToUpdate.Description = productEntity.Description ?? productToUpdate.Description;
            productToUpdate.Price = productEntity.Price;

            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> Delete(int id, CancellationToken cancellationToken)
        {
            bool isAdmin = await CurrentUserService.CheckAdmin(null, cancellationToken);
            
            if (isAdmin == false)
            {
                throw new UnauthorizedAccessException("You are not an admin");
            }
            
            var productToDelete = await WebShopApiContext.Products
                .Where(x => x.Id == id)
                .FirstOrNotFoundAsync(cancellationToken);
            
            WebShopApiContext.Products.Remove(productToDelete);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }
        
        private void ValidateCreateRequest(ProductDto data)
        {
            if (string.IsNullOrWhiteSpace(data.Name)
                || string.IsNullOrWhiteSpace(data.Description))
            {
                throw new ArgumentException();
            }
        }
    }
}