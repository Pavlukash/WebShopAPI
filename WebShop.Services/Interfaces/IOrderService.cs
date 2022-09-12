using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebShop.Domain.Models;

namespace WebShop.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetOrders(CancellationToken cancellationToken);
        Task<IEnumerable<OrderDto>> GetClientsOrders(int clientId, CancellationToken cancellationToken);
        Task<OrderDto> GetById(int id, CancellationToken cancellationToken);
        Task<OrderDto> Create(OrderDto orderEntity, CancellationToken cancellationToken);
        Task<bool> Delete(int id, CancellationToken cancellationToken);
    }
}