using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebShop.Domain.Entities;
using WebShop.Domain.Models;
using WebShop.Services.Extentions;
using WebShop.Services.Services;
using WebShop.UnitTests.Common;
using Xunit;

namespace WebShop.UnitTests.Tests
{
    public class ClientServiceTests : TestCommandBase
    {
        readonly ClientService _service;
        
        public ClientServiceTests(IHttpContextAccessor accessor)
        {
            var currentUserService = new CurrentUserService(Context, accessor);
            _service = new ClientService(Context, currentUserService);
        }

        [Fact]
        public void GetClients_NotNull()
        {
            var result = _service.GetClients(CancellationToken.None);
            
            Assert.NotNull(result);
        }
        
        [Fact]
        public void GetById_NotNull()
        {
            var result = _service.GetById(1, CancellationToken.None);
            
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task GiveDiscount_PropertiesValuesCreatedCorrectly()
        {
            await _service.GiveDiscount(1, 1, CancellationToken.None);
            
            var discount = await Context.Discounts
                .AsNoTracking()
                .Where(x => x.Id == 1)
                .FirstOrNotFoundAsync(CancellationToken.None);

            var client = await Context.Clients
                .AsNoTracking()
                .Where(x => x.Id == 1)
                .FirstOrNotFoundAsync(CancellationToken.None);
            
            var clientDiscount = await Context.ClientsDiscounts
                .AsNoTracking()
                .Where(x => x.ClientId == client.Id)
                .Where(x => x.DiscountId == discount.Id)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.Equal(client.Id, clientDiscount.ClientId);
            Assert.Equal(discount.Id, clientDiscount.DiscountId);
        }

        [Fact]
        public async Task Update_PropertiesValuesEditedCorrectly()
        {
            ClientDto editedDto = new ClientDto
            {
                Email = "edit",
                FirstName = "edit",
                LastName = "edit",
                PhoneNumber = "edit"
            };
            
            var result = await _service.Update(1, editedDto, CancellationToken.None);
            
            Assert.True(result);
            
            var client = await Context.Clients
                .AsNoTracking()
                .Where(x => x.Id == 1)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.Equal(editedDto.Email, client.Email);
            Assert.Equal(editedDto.FirstName, client.FirstName);
            Assert.Equal(editedDto.LastName, client.LastName);
            Assert.Equal(editedDto.LastName, client.PhoneNumber);
        }
        
        [Fact]
        public async Task Delete_ClientIsDeleted()
        {
            var result = await _service.Delete(1, CancellationToken.None);
            
            Assert.True(result);
            
            var client = await Context.Clients
                .AsNoTracking()
                .Where(x => x.Id == 1)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.Null(client);
        }
        
        [Fact]
        public async Task BanUser_ThrowsUserIsBannedException()
        {
             await Assert.ThrowsAsync<Exception>(() 
                 => _service.BanUser(3, CancellationToken.None));
        }
        
        [Fact]
        public async Task BanUser_SuccessfulBan()
        {
            var result = await _service.BanUser(2, CancellationToken.None);
            
            Assert.True(result);
            
            var client = await Context.Clients
                .AsNoTracking()
                .Where(x => x.Id == 2)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.True(client.IsBaned);
        }
        
        [Fact]
        public async Task UnbanUser_ThrowsUserIsNotBannedException()
        {
            await Assert.ThrowsAsync<Exception>(() 
                => _service.UnbanUser(2, CancellationToken.None));
        }
        
        [Fact]
        public async Task UnbanUser_SuccessfulUnban()
        {
            var result = await _service.UnbanUser(3, CancellationToken.None);
            
            Assert.True(result);
            
            var client = await Context.Clients
                .AsNoTracking()
                .Where(x => x.Id == 3)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.False(client.IsBaned);
        }
    }
}