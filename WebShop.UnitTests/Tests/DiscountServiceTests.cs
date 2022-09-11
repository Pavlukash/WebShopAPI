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
    public class DiscountServiceTests : TestCommandBase
    {
        private readonly DiscountService _service;

        public DiscountServiceTests()
        {
            _service = new DiscountService(Context);
        }

        [Fact]
        public async Task GetDiscounts_ThrowsNotAnAdminException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.GetDiscounts(false, CancellationToken.None));
        }
        
        [Fact]
        public async Task GetDiscounts_NotNull()
        {
            var result = await _service.GetDiscounts(true, CancellationToken.None);
            
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task GetClientsDiscounts_ThrowsNullReferenceException()
        {
            await Assert.ThrowsAsync<NullReferenceException>(() 
                => _service.GetClientsDiscounts(2, CancellationToken.None));
        }

        [Fact]
        public async Task GetClientsDiscounts_NotNull()
        {
            var result = await _service.GetClientsDiscounts(1, CancellationToken.None);
            
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task GetById_ThrowsNullReferenceException()
        {
            await Assert.ThrowsAsync<NullReferenceException>(() 
                => _service.GetById(2,true, CancellationToken.None));
        }
        
        [Fact]
        public async Task GetById_ThrowsNotAnAdminException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.GetById(2,false, CancellationToken.None));
        }
        
        [Fact]
        public async Task GetById_NotNull()
        {
            var result = await _service.GetById(1,true, CancellationToken.None);
            
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task Create_NotNull()
        {
            var newDiscountDto = new DiscountDto
            {
               ProductId = 1,
               ClientId = 2,
               Discount = 50
            };
            
            Assert.NotNull(await _service.Create(newDiscountDto, true, CancellationToken.None));
        }
        
        [Fact]
        public async Task Create_ThrowsNotAnAdminException()
        {
            var newDiscountDto = new DiscountDto
            {
                ProductId = 1,
                Discount = 50
            };
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.Create(newDiscountDto,false, CancellationToken.None));
        }

        [Fact]
        public async Task Create_PropertiesValuesCreatedCorrectly()
        {
            var newDiscountDto = new DiscountDto
            {
                ProductId = 1,
                Discount = 50
            };

            await _service.Create(newDiscountDto, true, CancellationToken.None);
            
            var discount = await Context.Discounts
                .AsNoTracking()
                .Where(x => x.ProductId == newDiscountDto.ProductId)
                .Where(x => x.Discount == newDiscountDto.Discount)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.Equal(newDiscountDto.ProductId, discount.ProductId);
            Assert.Equal(newDiscountDto.Discount, discount.Discount);
        }
        
        [Fact]
        public async Task Update_ThrowsNotAnAdminException()
        {
            var editedDto = new DiscountDto
            {
                ProductId = 2,
                Discount = 25
            };
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.Update(1, editedDto,false, CancellationToken.None));
        }
        
        [Fact]
        public async Task Update_ThrowsNullReferenceException()
        {
            var editedDto = new DiscountDto
            {
                ProductId = 2,
                Discount = 25
            };
            
            await Assert.ThrowsAsync<NullReferenceException>(() 
                => _service.Update(1000, editedDto, true, CancellationToken.None));
        }

        [Fact]
        public async Task Update_PropertiesValuesEditedCorrectly()
        {
            var editedDto = new DiscountDto
            {
                ProductId = 3,
                Discount = 30
            };

            var result = await _service.Update(1, editedDto, true, CancellationToken.None);

            Assert.True(result);

            var discount = await Context.Discounts
                .AsNoTracking()
                .Where(x => x.ProductId == editedDto.ProductId)
                .Where(x => x.Discount == editedDto.Discount)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.Equal(editedDto.ProductId, discount.ProductId);
            Assert.Equal(editedDto.Discount, discount.Discount);
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
        public async Task Delete_DiscountIsDeleted()
        {
            var result = await _service.Delete(1, true, CancellationToken.None);
            
            Assert.True(result);
            
            var discount = await Context.Discounts
                .AsNoTracking()
                .Where(x => x.Id == 1)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.Null(discount);
        }
    }
}