using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShop.Domain.Models;
using WebShop.Services.Services;
using WebShop.UnitTests.Common;
using Xunit;

namespace WebShop.UnitTests.Tests
{
    public class ProductServiceTests : TestCommandBase
    {
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            var clientService = new ClientService(Context);
            _service = new ProductService(Context, clientService);
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
        public async Task GetById_ThrowsNullReferenceException()
        {
            await Assert.ThrowsAsync<NullReferenceException>(()
                => _service.GetById(1000, CancellationToken.None));
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
            
            Assert.NotNull(await _service.Create(newProductDto, true, CancellationToken.None));
        }
        
        [Fact]
        public async Task Create_ThrowsNotAnAdminException()
        {
            var newProductDto = new ProductDto
            {
                Name = "NewProduct",
                Description = "NewProduct",
                Price = 100
            };
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.Create(newProductDto,false, CancellationToken.None));
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

            await _service.Create(newProductDto, true, CancellationToken.None);
            
            var product = await Context.Products
                .AsNoTracking()
                .Where(x => x.Name == newProductDto.Name)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.Equal(newProductDto.Name, product.Name);
            Assert.Equal(newProductDto.Description, product.Description);
            Assert.Equal(newProductDto.Price, product.Price);
        }
        
        [Fact]
        public async Task Update_ThrowsNotAnAdminException()
        {
            ProductDto editedDto = new ProductDto
            {
                Name = "edit",
                Description = "edit",
                Price = 200
            };
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.Create(editedDto,false, CancellationToken.None));
        }
        
        [Fact]
        public async Task Update_ThrowsNullReferenceException()
        {
            ProductDto editedDto = new ProductDto
            {
                Name = "edit",
                Description = "edit",
                Price = 200
            };
            
            await Assert.ThrowsAsync<NullReferenceException>(() 
                => _service.Update(1000, editedDto, true, CancellationToken.None));
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
            
            var result = await _service.Update(1, editedDto, true, CancellationToken.None);
            
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
        public async Task Delete_ThrowsNotAnAdminException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.Delete(1,false, CancellationToken.None));
        }
        
        [Fact]
        public async Task Delete_ThrowsNullReferenceException()
        {
            await Assert.ThrowsAsync<NullReferenceException>(() 
                => _service.Delete(1000, true, CancellationToken.None));
        }

        [Fact]
        public async Task Delete_ProductIsDeleted()
        {
            var result = await _service.Delete(1, true, CancellationToken.None);
            
            Assert.True(result);
            
            var product = await Context.Products
                .AsNoTracking()
                .Where(x => x.Id == 1)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.Null(product);
        }
    }
}