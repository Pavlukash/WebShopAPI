using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebShop.Domain.Models;

namespace WebShop.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetOrders(bool isAdmin, CancellationToken cancellationToken);
        Task<IEnumerable<OrderDto>> GetClientOrders(int clientId, CancellationToken cancellationToken);
        Task<OrderDto> Get(int id, CancellationToken cancellationToken);
        Task<OrderDto> Create(OrderDto orderEntity, CancellationToken cancellationToken);
        Task<bool> Update(int id, OrderDto orderEntity, CancellationToken cancellationToken);
        Task<bool> Delete(int id, CancellationToken cancellationToken);
    }
}