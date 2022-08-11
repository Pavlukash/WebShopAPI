using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShop.Domain.Contexts;
using WebShop.Domain.Entities;
using WebShop.Domain.Models;
using WebShop.Services.Interfaces;
using WebShop.Services.Mappers;

namespace WebShop.Services.Services
{
    public class ProductService : IProductService
    {
        private WebShopApiContext WebShopApiContext { get; }

        public ProductService(WebShopApiContext context)
        {
            WebShopApiContext = context;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts(CancellationToken cancellationToken)
        {
            var products = await WebShopApiContext.Products
                .AsNoTracking()
                .Select(x => x.ToDto())
                .ToListAsync(cancellationToken);

            return products;
        }
        
        public async Task<ProductDto> Get(int id, CancellationToken cancellationToken)
        {
            var product = await WebShopApiContext.Products
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                throw new Exception();
            }

            var result = product.ToDto();

            return result;
        }

        public async Task<ProductDto> Create(ProductDto newProductEntity, CancellationToken cancellationToken)
        {
            ValidateCreateRequest(newProductEntity);
            
            var newEntity = new ProductEntity()
            {
                Name = newProductEntity.Name,
                Description = newProductEntity.Description,
                Price = newProductEntity.Price
            };

            WebShopApiContext.Products.Add(newEntity);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            var result = newEntity.ToDto();

            return result;
        }

        public async Task<bool> Update(int id, ProductDto productEntity, CancellationToken cancellationToken)
        {
            var productToUpdate = await WebShopApiContext.Products
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (productToUpdate == null)
            {
                throw new Exception();
            }

            productToUpdate.Name = productEntity.Name ?? productToUpdate.Name;
            productToUpdate.Description = productEntity.Description ?? productToUpdate.Description;
            productToUpdate.Price = productEntity.Price ?? productToUpdate.Price;

            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> Delete(int id, CancellationToken cancellationToken)
        {
            var productToDelete = await WebShopApiContext.Products
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

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