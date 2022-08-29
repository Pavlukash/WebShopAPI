using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebShop.Common.Options;
using WebShop.Common.Responses;
using WebShop.Domain.Contexts;
using WebShop.Domain.Entities;
using WebShop.Domain.Models;
using WebShop.Services.Auth;
using WebShop.Services.Interfaces;
using WebShop.Services.Mappers;

namespace WebShop.Services.Services
{
    public class JwtService : IJwtService
    {
        private WebShopApiContext WebShopApiContext { get; }
        private IClientService ClientService { get; }

        public JwtService(WebShopApiContext context, IClientService clientService)
        {
            WebShopApiContext = context;
            ClientService = clientService;
        }
        
        public async Task<LoginResponse> Login(string email, string password, CancellationToken cancellationToken)
        {
            var identity = await GetIdentity(email, password, cancellationToken);
            
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
 
            var now = DateTime.UtcNow;
            
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.Issuer,
                audience: AuthOptions.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            LoginResponse response = new LoginResponse()
            {
                Token = encodedJwt,
                UserName = identity.Name
            };
            
            return response;
        }

        private static LoginResponse BadRequest(object o)
        {
            throw new NotImplementedException();
        }

        private async Task<ClaimsIdentity> GetIdentity(string email, string password, CancellationToken cancellationToken)
        {
            var user = await ClientService.GetByEmailAndPassword(email, password, cancellationToken);

            if (user.IsBaned)
            {
                throw new Exception();
            }
            
            var userRole = await GetRoleById(user.Id, cancellationToken);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email ?? throw new InvalidOperationException()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole.Name)
            };
            
            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            
            return claimsIdentity;
        }

        public async Task<RoleDto> GetRoleById(int id, CancellationToken cancellationToken)
        {
            var role = await WebShopApiContext.Roles
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (role == null)
            {
                throw new Exception();
            }

            var result = role.ToDto();

            return result;
        }

        public async Task<ClientDto> Register(ClientDto newClientEntity, CancellationToken cancellationToken)
        {
            ValidateRegisterRequest(newClientEntity);

            var newEntity = new ClientEntity()
            {
                Email = newClientEntity.Email,
                Password = PasswordHasher.HashPassword(newClientEntity.Password),
                PhoneNumber = newClientEntity.PhoneNumber,
                FirstName = newClientEntity.FirstName,
                LastName = newClientEntity.LastName,
                RoleId = 2
            };

            WebShopApiContext.Clients.Add(newEntity);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            var result = newEntity.ToDto();

            return result;
        }
        
        private void ValidateRegisterRequest(ClientDto data)
        {
            if (string.IsNullOrWhiteSpace(data.FirstName)
                || string.IsNullOrWhiteSpace(data.LastName)
                || string.IsNullOrWhiteSpace(data.Password))
            {
                throw new ArgumentException();
            }
        }
    }
}