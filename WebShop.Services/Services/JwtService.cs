using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebShop.Common.Auth;
using WebShop.Common.Options;
using WebShop.Common.Responses;
using WebShop.Domain.Contexts;
using WebShop.Domain.Entities;
using WebShop.Domain.Models;
using WebShop.Services.Extentions;
using WebShop.Services.Interfaces;
using WebShop.Services.Mappers;

namespace WebShop.Services.Services
{
    public class JwtService : IJwtService
    {
        private WebShopApiContext WebShopApiContext { get; }
        private IPasswordHandler PasswordHandler { get; }

        public JwtService(WebShopApiContext context, IPasswordHandler passwordHandler)
        {
            WebShopApiContext = context;
            PasswordHandler = passwordHandler;
        }

        public async Task<LoginResponse> Login(string email, string password, CancellationToken cancellationToken)
        {
            var identity = await GetIdentity(email, password, cancellationToken);
            
            if (identity == null)
            {
                throw new ArgumentException("Invalid username or password.");
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

            LoginResponse response = new LoginResponse
            {
                Token = encodedJwt,
                UserName = identity.Name
            };
            
            return response;
        }

        private async Task<ClaimsIdentity> GetIdentity(string email, string password, CancellationToken cancellationToken)
        {
            var user = await GetByEmailAndPassword(email, password, cancellationToken);

            if (user.IsBaned)
            {
                throw new Exception();
            }
            
            var userRole = await GetRoleById(user.RoleId, cancellationToken);
            
            var claims = new List<Claim>
            {
                new (ClaimsIdentity.DefaultNameClaimType, user.Email!),
                new (ClaimsIdentity.DefaultRoleClaimType, userRole.Name)
            };
            
            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        private async Task<ClientDto> GetByEmailAndPassword(string email, string password, CancellationToken cancellationToken)
        {
            var client = await WebShopApiContext.Clients
                .AsNoTracking()
                .Where(x => x.Email == email)
                .FirstOrNotFoundAsync(cancellationToken);

            var passwordIsValid = PasswordHandler.IsValid(password, client.PasswordHash, client.PasswordSalt);

            if (!passwordIsValid)
            {
                throw new Exception();
            }
            
            var result = client.ToDto();

            return result;
        }

        private async Task<RoleDto> GetRoleById(int id, CancellationToken cancellationToken)
        {
            var role = await WebShopApiContext.Roles
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrNotFoundAsync(cancellationToken);

            var result = role.ToDto();

            return result;
        }

        public async Task<ClientDto> Register(ClientDto newClientDto, CancellationToken cancellationToken)
        {
            ValidateRegisterRequest(newClientDto);

            var isExists = await WebShopApiContext.Clients
                .Where(x => x.Email == newClientDto.Email)
                .AnyAsync(cancellationToken);

            if (isExists)
            {
                throw new Exception("User already exists");
            }
            
            PasswordHandler.CreateHash(newClientDto.Password, out string hash, out string salt);

            var newEntity = new ClientEntity()
            {
                Email = newClientDto.Email!,
                PasswordHash = hash,
                PasswordSalt = salt,
                PhoneNumber = newClientDto.PhoneNumber,
                FirstName = newClientDto.FirstName!,
                LastName = newClientDto.LastName!,
                RoleId = newClientDto.RoleId
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
                throw new ArgumentException("Invalid user data");
            }
        }
    }
}