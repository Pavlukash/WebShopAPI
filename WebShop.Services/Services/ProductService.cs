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
        
        private IClientService ClientService { get; }

        public ProductService(WebShopApiContext context, IClientService clientService)
        {
            WebShopApiContext = context;
            ClientService = clientService;
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
                .FirstOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                throw new NullReferenceException();
            }
            
            var result = product.ToDto();
            
            var discount = await WebShopApiContext.Discounts
                .AsNoTracking()
                .Where(x => x.ClientId == 1)
                .Where(x => x.ProductId == product.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (discount != null)
            {
                result.NewPrice = product.Price * (1 - discount.Discount / 100);
            }

            return result;
        }

        public async Task<bool> AddToProductList(int id, CancellationToken cancellationToken)
        {
            var product = await WebShopApiContext.Products
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (product == null)
            {
                throw new NullReferenceException();
            }
            
            var client = await WebShopApiContext.Clients
                .AsNoTracking()
                .Where(x => x.Id == 1)
                .Include(x => x.ProductList)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (client == null)
            {
                throw new NullReferenceException();
            }

            if (client.ProductList == null)
            {
                client.ProductList = new List<ProductEntity>();
            }
            client.ProductList.Add(product);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> RemoveFromProductList(int id, CancellationToken cancellationToken)
        {
            var product = await WebShopApiContext.Products
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (product == null)
            {
                throw new NullReferenceException();
            }
            
            var client = await WebShopApiContext.Clients
                .AsNoTracking()
                .Include(x => x.ProductList)
                .Where(x => x.Id == 1)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (client == null)
            {
                throw new NullReferenceException();
            }

            if (client.ProductList == null)
            {
                throw new NullReferenceException();
            }
            
            client.ProductList.Remove(product);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<ProductDto> Create(ProductDto newProductEntity, bool isAdmin, CancellationToken cancellationToken)
        {
            ValidateCreateRequest(newProductEntity);

            if (isAdmin == false)
            {
                throw new ArgumentException("You are not an admin");
            }
            
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

        public async Task<bool> Update(int id, ProductDto productEntity, bool isAdmin, CancellationToken cancellationToken)
        {
            var productToUpdate = await WebShopApiContext.Products
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (isAdmin == false)
            {
                throw new ArgumentException("You are not an admin");
            }
            
            if (productToUpdate == null)
            {
                throw new NullReferenceException();
            }

            productToUpdate.Name = productEntity.Name ?? productToUpdate.Name;
            productToUpdate.Description = productEntity.Description ?? productToUpdate.Description;
            productToUpdate.Price = productEntity.Price;

            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> Delete(int id, bool isAdmin, CancellationToken cancellationToken)
        {
            var productToDelete = await WebShopApiContext.Products
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (isAdmin == false)
            {
                throw new ArgumentException("You are not an admin");
            }

            if (productToDelete == null)
            {
                throw new NullReferenceException();
            }

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