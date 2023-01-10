using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        public OrderServiceTests(IHttpContextAccessor accessor)
        {
            var currentUserService = new CurrentUserService(Context, accessor);
            _service = new OrderService(Context, currentUserService);
        }

        [Fact]
        public async Task GetOrders_NotNull()
        {
            var result = await _service.GetOrders(CancellationToken.None);
            
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task GetClientsOrders_NotNull()
        {
            var result = await _service.GetClientsOrders(1, CancellationToken.None);
            
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task GetById_NotNull()
        {
            var result = await _service.GetById(1, CancellationToken.None);
            
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task Create_NotNull()
        {
            var result = await _service.Create(new OrderDto(), CancellationToken.None);
            
            Assert.NotNull(result);
        } 
        
        [Fact]
        public async Task Create_PropertiesValuesCreatedCorrectly()
        {
            await _service.Create(new OrderDto(), CancellationToken.None);

            var order = await Context.Orders
                .AsNoTracking()
                .Where(x => x.Id == 1)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.Equal(1, order.ClientId);
            Assert.Equal(275, order.TotalPrice);
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