using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public CurrentUserService(WebShopApiContext webShopApiContext)
        {
            WebShopApiContext = webShopApiContext;
        }
        
        public async Task<ClientEntity> GetCurrentUser(CancellationToken cancellationToken)
        {
            var result = await WebShopApiContext.Clients
                .AsNoTracking()
                .Where(x => x.Id == 1)
                .FirstOrNotFoundAsync(cancellationToken);

            return result;
        }

        public async Task<bool> CheckAdmin(ClientEntity? currentUser, CancellationToken cancellationToken)
        {
            currentUser ??= await GetCurrentUser(cancellationToken);
            
            return currentUser.RoleId == 1;
        }
    }
}