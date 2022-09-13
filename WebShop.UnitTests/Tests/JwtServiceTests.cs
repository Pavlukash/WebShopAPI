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
    public class JwtServiceTests : TestCommandBase
    {
        readonly JwtService _jwtService;
        
        public JwtServiceTests()
        {
            _jwtService = new JwtService(Context);
        }

        [Fact]
        public async Task Login_ResponseNotNull()
        {
            var result = await _jwtService.Login("admin", "admin", CancellationToken.None);
            
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task Register_NotNull()
        {
            var newClientDto = new ClientDto()
            {
                Email = "NewUser",
                Password = "NewUser",
                PhoneNumber = "NewUser",
                FirstName = "NewUser",
                LastName = "NewUser",
                RoleId = 2 
            };
            
            Assert.NotNull(await _jwtService.Register(newClientDto, CancellationToken.None));
        }
        
        [Fact]
        public async Task Register_ThrowsNullReferenceException()
        {
            var newClientDto = new ClientDto
            {
                Email = "NewUser",
                Password = "",
                PhoneNumber = "NewUser",
                FirstName = "NewUser",
                LastName = "",
                RoleId = 2 
            };
            
            await Assert.ThrowsAsync<ArgumentException>(() 
                => _jwtService.Register(newClientDto, CancellationToken.None));
        }

        [Fact]
        public async Task Register_PropertiesValuesCreatedCorrectly()
        {
            var newClientDto = new ClientDto()
            {
                Email = "NewUser",
                Password = "NewUser",
                PhoneNumber = "NewUser",
                FirstName = "NewUser",
                LastName = "NewUser",
                RoleId = 2 
            };

            await _jwtService.Register(newClientDto, CancellationToken.None);
            
            var client = await Context.Clients
                .AsNoTracking()
                .Where(x => x.Email == newClientDto.Email)
                .FirstOrDefaultAsync(CancellationToken.None);
            
            Assert.Equal(newClientDto.Email, client.Email);
            Assert.Equal(newClientDto.Password, client.Password);
            Assert.Equal(newClientDto.FirstName, client.FirstName);
            Assert.Equal(newClientDto.LastName, client.LastName);
            Assert.Equal(newClientDto.LastName, client.PhoneNumber);
        }
    }
}