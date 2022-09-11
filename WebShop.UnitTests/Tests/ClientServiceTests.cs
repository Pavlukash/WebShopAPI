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
    public class ClientServiceTests : TestCommandBase
    {
        readonly ClientService _service;
        
        public ClientServiceTests()
        {
            _service = new ClientService(Context);
        }

        [Fact]
        public void GetClients_NotNull()
        {
            var result = _service.GetClients(true, CancellationToken.None);
            
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task GetClients_ThrowsNotAnAdminException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.GetClients(false, CancellationToken.None));
        }
        
        [Fact]
        public void GetById_NotNull()
        {
            var result = _service.GetById(1,true, CancellationToken.None);
            
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetById_ThrowsNullReferenceException()
        {
            await Assert.ThrowsAsync<NullReferenceException>(()
                => _service.GetById(1000, true, CancellationToken.None));
        }

        [Fact]
        public async Task GetById_ThrowsNotAnAdminException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.GetById(1,false, CancellationToken.None));
        }

        [Fact]
        public async Task Update_ThrowsNullReferenceException()
        {
            ClientDto editedDto = new ClientDto
            {
                Email = "edit",
                FirstName = "edit",
                LastName = "edit",
                PhoneNumber = "edit"
            };
            
            await Assert.ThrowsAsync<NullReferenceException>(() 
                => _service.Update(1000, editedDto, CancellationToken.None));
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
        public async Task Delete_ThrowsNullReferenceException()
        {
            var exception = await Assert.ThrowsAsync<NullReferenceException>(() 
                => _service.Delete(1000, CancellationToken.None));
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
        public async Task BanUser_ThrowsNotAnAdminException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.BanUser(1,false, CancellationToken.None));
        }
        
        [Fact]
        public async Task BanUser_ThrowsNullReferenceException()
        {
            await Assert.ThrowsAsync<NullReferenceException>(() 
                => _service.BanUser(1000, true, CancellationToken.None));
        }
        
        [Fact]
        public async Task BanUser_ThrowsUserIsBannedException()
        {
             await Assert.ThrowsAsync<Exception>(() 
                 => _service.BanUser(3, true, CancellationToken.None));
        }
        
        [Fact]
        public async Task BanUser_SuccessfulBan()
        {
            var result = await _service.BanUser(2, true, CancellationToken.None);
            
            Assert.True(result);
            
            var client = await Context.Clients
                .AsNoTracking()
                .Where(x => x.Id == 2)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.True(client.IsBaned);
        }
        
        [Fact]
        public async Task UnbanUser_ThrowsNotAnAdminException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _service.UnbanUser(1,false, CancellationToken.None));
        }

        [Fact]
        public async Task UnbanUser_ThrowsNullReferenceException()
        {
            await Assert.ThrowsAsync<NullReferenceException>(()
                => _service.UnbanUser(1000, true, CancellationToken.None));
        }

        [Fact]
        public async Task UnbanUser_ThrowsUserIsNotBannedException()
        {
            await Assert.ThrowsAsync<Exception>(() 
                => _service.UnbanUser(2, true, CancellationToken.None));
        }
        
        [Fact]
        public async Task UnbanUser_SuccessfulUnban()
        {
            var result = await _service.UnbanUser(3, true, CancellationToken.None);
            
            Assert.True(result);
            
            var client = await Context.Clients
                .AsNoTracking()
                .Where(x => x.Id == 3)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.False(client.IsBaned);
        }
    }
}