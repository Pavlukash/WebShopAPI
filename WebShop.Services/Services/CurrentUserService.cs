using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebShop.Domain.Contexts;
using WebShop.Domain.Entities;
using WebShop.Services.Extentions;
using WebShop.Services.Interfaces;

namespace WebShop.Services.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private WebShopApiContext WebShopApiContext { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }

        public CurrentUserService(WebShopApiContext webShopApiContext, IHttpContextAccessor httpContextAccessor)
        {
            WebShopApiContext = webShopApiContext;
            HttpContextAccessor = httpContextAccessor;
        }
        
        public async Task<ClientEntity> GetCurrentUser(CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            
            var result = await WebShopApiContext.Clients
                .AsNoTracking()
                .Where(x => x.Id == userId)
                .FirstOrNotFoundAsync(cancellationToken);

            return result;
        }
        
        public int GetCurrentUserId()
        {
            var claimStr = HttpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value;
            if (!int.TryParse(claimStr, out var userId))
            {
                throw new Exception();
            }

            return userId;
        }

        public async Task<bool> CheckAdmin(ClientEntity? currentUser, CancellationToken cancellationToken)
        {
            currentUser ??= await GetCurrentUser(cancellationToken);
            
            return currentUser.RoleId == 1;
        }
    }
}