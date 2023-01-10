using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebShop.Domain.Models;
using WebShop.Services.Extentions;
using WebShop.Services.Services;
using WebShop.UnitTests.Common;
using Xunit;

namespace WebShop.UnitTests.Tests
{
    public class ProductServiceTests : TestCommandBase
    {
        private readonly ProductService _service;

        public ProductServiceTests(IHttpContextAccessor accessor)
        {
            var currentUserService = new CurrentUserService(Context, accessor);
            _service = new ProductService(Context, currentUserService);
        }
        
        [Fact]
        public async Task GetProducts_NotNull()
        {
            var result = await _service.GetProducts(CancellationToken.None);
            
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task GetById_NotNull()
        {
            var result = await _service.GetById(1, CancellationToken.None);
            
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetById_NewPriceIsCorrect()
        {
            var result = await _service.GetById(2, CancellationToken.None);
            
            Assert.Equal(75, result.NewPrice);
        }
        
        [Fact]
        public async Task AddToProductList_PropertiesValuesCreatedCorrectly()
        {
            var result = await _service.AddToProductList(1, CancellationToken.None);
            
            Assert.True(result);
            
            var product = await Context.Products
                .AsNoTracking()
                .Where(x => x.Id == 1)
                .FirstOrNotFoundAsync(CancellationToken.None);

            var client = await Context.Clients
                .AsNoTracking()
                .Where(x => x.Id == 1)
                .FirstOrNotFoundAsync(CancellationToken.None);
            
            var clientProduct = await Context.ClientsProducts
                .AsNoTracking()
                .Where(x => x.ClientId == client.Id)
                .Where(x => x.ProductId == product.Id)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.Equal(client.Id, clientProduct.ClientId);
            Assert.Equal(product.Id, clientProduct.ProductId);
        }

        [Fact]
        public async Task RemoveFromProductList_ProductIsDeleted()
        {
            //await _service.AddToProductList(1, CancellationToken.None);
            
            var result = await _service.RemoveFromProductList(1, CancellationToken.None);
            
            Assert.True(result);
            
            var product = await Context.Products
                .AsNoTracking()
                .Where(x => x.Id == 1)
                .FirstOrDefaultAsync(CancellationToken.None);

            var client = await Context.Clients
                .AsNoTracking()
                .Where(x => x.Id == 1)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            var clientProduct = await Context.ClientsProducts
                .AsNoTracking()
                .Where(x => x.ClientId == client.Id)
                .Where(x => x.ProductId == product.Id)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.Null(clientProduct);
        }
        
        [Fact]
        public async Task Create_NotNull()
        {
            var newProductDto = new ProductDto
            {
                Name = "NewProduct",
                Description = "NewProduct",
                Price = 100
            };
            
            Assert.NotNull(await _service.Create(newProductDto, CancellationToken.None));
        }
        
        [Fact]
        public async Task Create_PropertiesValuesCreatedCorrectly()
        {
            var newProductDto = new ProductDto
            {
                Name = "NewProduct",
                Description = "NewProduct",
                Price = 100
            };

            await _service.Create(newProductDto, CancellationToken.None);
            
            var product = await Context.Products
                .AsNoTracking()
                .Where(x => x.Name == newProductDto.Name)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.Equal(newProductDto.Name, product.Name);
            Assert.Equal(newProductDto.Description, product.Description);
            Assert.Equal(newProductDto.Price, product.Price);
        }

        [Fact]
        public async Task Update_PropertiesValuesEditedCorrectly()
        {
            ProductDto editedDto = new ProductDto
            {
                Name = "edit",
                Description = "edit",
                Price = 200
            };
            
            var result = await _service.Update(1, editedDto, CancellationToken.None);
            
            Assert.True(result);
            
            var product = await Context.Products
                .AsNoTracking()
                .Where(x => x.Id == 1)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.Equal(editedDto.Name, product.Name);
            Assert.Equal(editedDto.Description, product.Description);
            Assert.Equal(editedDto.Price, product.Price);
        }
        
        [Fact]
        public async Task Delete_ProductIsDeleted()
        {
            var result = await _service.Delete(1, CancellationToken.None);
            
            Assert.True(result);
            
            var product = await Context.Products
                .AsNoTracking()
                .Where(x => x.Id == 1)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.Null(product);
        }
    }
}