using System.Threading;
using System.Threading.Tasks;
using WebShop.Domain.Entities;
using WebShop.Domain.Models;

namespace WebShop.Services.Interfaces
{
    public interface ICurrentUserService
    {
        Task<ClientEntity> GetCurrentUser(CancellationToken cancellationToken);
        Task<bool> CheckAdmin(ClientEntity? currentUser, CancellationToken cancellationToken);
    }
}