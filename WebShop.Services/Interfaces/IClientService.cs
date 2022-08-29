using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebShop.Domain.Models;

namespace WebShop.Services.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetClients(bool isAdmin, CancellationToken cancellationToken);
        Task<ClientDto> GetById(int id, bool isAdmin, CancellationToken cancellationToken);
        Task<ClientDto> GetByEmailAndPassword(string email, string password, CancellationToken cancellationToken);
        Task<bool> Update(int id, ClientDto clientEntity, CancellationToken cancellationToken);
        Task<bool> Delete(int id, CancellationToken cancellationToken);
        Task<bool> BanUser(int id, bool isAdmin, CancellationToken cancellationToken);
        Task<bool> UnbanUser(int id, bool isAdmin, CancellationToken cancellationToken);
    }
}