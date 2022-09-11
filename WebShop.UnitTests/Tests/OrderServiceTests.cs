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
    public class OrderServiceTests : TestCommandBase
    {
         private readonly OrderService _service;

        public OrderServiceTests()
        {
            _service = new OrderService(Context);
        }

        [Fact]
        public async Task GetDiscounts_ThrowsNotAnAdminException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.GetOrders(false, CancellationToken.None));
        }
        
        [Fact]
        public async Task GetOrders_NotNull()
        {
            var result = await _service.GetOrders(true, CancellationToken.None);
            
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task GetClientsOrders_NotNull()
        {
            var result = await _service.GetClientsOrders(1, CancellationToken.None);
            
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task GetById_ThrowsNullReferenceException()
        {
            await Assert.ThrowsAsync<NullReferenceException>(() 
                => _service.GetById(1000, CancellationToken.None));
        }
        
        [Fact]
        public async Task GetById_NotNull()
        {
            var result = await _service.GetById(1, CancellationToken.None);
            
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task Update_ThrowsNullReferenceException()
        {
            var editedDto = new OrderDto
            {
                TotalPrice = 10
            };
            
            await Assert.ThrowsAsync<NullReferenceException>(() 
                => _service.Update(1000, editedDto, CancellationToken.None));
        }

        [Fact]
        public async Task Update_PropertiesValuesEditedCorrectly()
        {
            var editedDto = new OrderDto
            {
                TotalPrice = 10
            };

            var result = await _service.Update(1, editedDto, CancellationToken.None);

            Assert.True(result);

            var order = await Context.Orders
                .AsNoTracking()
                .Where(x => x.Id == 1)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.Equal(editedDto.TotalPrice, order.TotalPrice);
        }
        
        [Fact]
        public async Task Delete_ThrowsNullReferenceException()
        {
            await Assert.ThrowsAsync<NullReferenceException>(() 
                => _service.Delete(1000,  CancellationToken.None));
        }

        [Fact]
        public async Task Delete_OrderIsDeleted()
        {
            var result = await _service.Delete(1, CancellationToken.None);
            
            Assert.True(result);
            
            var discount = await Context.Orders
                .AsNoTracking()
                .Where(x => x.Id == 1)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.Null(discount);
        }
    }
}