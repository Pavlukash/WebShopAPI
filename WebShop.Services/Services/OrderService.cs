using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebShop.Domain.Contexts;
using WebShop.Domain.Entities;
using WebShop.Domain.Models;
using WebShop.Services.Interfaces;
using WebShop.Services.Mappers;
using Microsoft.EntityFrameworkCore;

namespace WebShop.Services.Services
{
    public class OrderService : IOrderService
    {
        private WebShopApiContext WebShopApiContext { get; }

        public OrderService(WebShopApiContext context)
        {
            WebShopApiContext = context;
        }

        public async Task<IEnumerable<OrderDto>> GetOrders(bool isAdmin, CancellationToken cancellationToken)
        {
            var orders = await WebShopApiContext.Orders
                .AsNoTracking()
                .Select(x => x.ToDto())
                .ToListAsync(cancellationToken);
            
            if (isAdmin == false)
            {
                throw new Exception();
            }

            return orders;
        }
        
        public async Task<IEnumerable<OrderDto>> GetClientOrders(int clientId, CancellationToken cancellationToken)
        {
            var clientOrders = await WebShopApiContext.Orders
                .AsNoTracking()
                .Where(x => x.ClientId == clientId)
                .Select(x => x.ToDto())
                .ToListAsync(cancellationToken);

            return clientOrders;
        }

        public async Task<OrderDto> Get(int id, CancellationToken cancellationToken)
        {
            var order = await WebShopApiContext.Orders
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (order == null)
            {
                throw new Exception();
            }

            var result = order.ToDto();

            return result;
        }

        public async Task<OrderDto> Create(OrderDto newOrderEntity, CancellationToken cancellationToken)
        {
            var newEntity = new OrderEntity()
            {
                ClientId = newOrderEntity.ClientId,
                TotalPrice = newOrderEntity.TotalPrice
            };

            WebShopApiContext.Orders.Add(newEntity);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            var result = newEntity.ToDto();

            return result;
        }

        public async Task<bool> Update(int id, OrderDto orderEntity, CancellationToken cancellationToken)
        {
            var orderToUpdate = await WebShopApiContext.Orders
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (orderToUpdate == null)
            {
                throw new Exception();
            }

            orderToUpdate.TotalPrice = orderEntity.TotalPrice ?? orderToUpdate.TotalPrice;

            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> Delete(int id, CancellationToken cancellationToken)
        {
            var orderToDelete = await WebShopApiContext.Orders
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            WebShopApiContext.Orders.Remove(orderToDelete);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}