using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebShop.Domain.Models;

namespace WebShop.Services.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetClients(CancellationToken cancellationToken);
        Task<ClientDto> Get(int id, CancellationToken cancellationToken);
        Task<ClientDto> Create(ClientDto clientEntity, CancellationToken cancellationToken);
        Task<bool> Update(int id, ClientDto clientEntity, CancellationToken cancellationToken);
        Task<bool> Delete(int id, CancellationToken cancellationToken);
    }
}