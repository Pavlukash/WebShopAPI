using System.Threading;
using System.Threading.Tasks;
using WebShop.Domain.Entities;

namespace WebShop.Services.Interfaces
{
    public interface ICurrentUserService
    {
        Task<ClientEntity> GetCurrentUser(CancellationToken cancellationToken);
        public int GetCurrentUserId();
        Task<bool> CheckAdmin(ClientEntity? currentUser, CancellationToken cancellationToken);
    }
}