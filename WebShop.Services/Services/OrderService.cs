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
                throw new ArgumentException("You are not an admin");
            }
            
            return orders;
        }
        
        public async Task<IEnumerable<OrderDto>> GetClientsOrders(int clientId, CancellationToken cancellationToken)
        {
            var clientOrders = await WebShopApiContext.Orders
                .AsNoTracking()
                .Where(x => x.ClientId == clientId)
                .Select(x => x.ToDto())
                .ToListAsync(cancellationToken);

            return clientOrders;
        }

        public async Task<OrderDto> GetById(int id, CancellationToken cancellationToken)
        {
            var order = await WebShopApiContext.Orders
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (order == null)
            {
                throw new NullReferenceException();
            }

            var result = order.ToDto();

            return result;
        }

        public async Task<OrderDto> Create(OrderDto newOrderEntity, CancellationToken cancellationToken)
        {
            var newEntity = new OrderEntity()
            {
                ClientId = newOrderEntity.ClientId,
                TotalPrice = await CalculateTotalPrice(newOrderEntity.ClientId, cancellationToken)
            };

            WebShopApiContext.Orders.Add(newEntity);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            var result = newEntity.ToDto();

            return result;
        }

        private async Task<decimal?> CalculateTotalPrice(int id, CancellationToken cancellationToken)
        {
            var client = await WebShopApiContext.Clients
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Include(x => x.ProductList)
                .FirstOrDefaultAsync(cancellationToken);

            if (client == null)
            {
                throw new NullReferenceException();
            }

            decimal? result = 0;

            if (client.ProductList?.Any() != true)
            {
                throw new Exception();
            }
            
            var someClientDiscount = await WebShopApiContext.Discounts
                .AsNoTracking()
                .Where(x => x.ClientId == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (someClientDiscount == null)
            {
                foreach (var product in client.ProductList)
                {
                    result += product.Price;
                }
            }
            else
            {
                foreach (var product in client.ProductList)
                {
                    var discount = await WebShopApiContext.Discounts
                        .AsNoTracking()
                        .Where(x => x.ClientId == id)
                        .Where(x => x.ProductId == product.Id)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (discount == null)
                    {
                        result += product.Price;
                    }
                    else
                    {
                        result += product.Price * (1 - discount.Discount / 100);
                    }
                }
            }

            if (result == null)
            {
                throw new NullReferenceException();
            }

            return result;
        }

        public async Task<bool> Update(int id, OrderDto orderEntity, CancellationToken cancellationToken)
        {
            var orderToUpdate = await WebShopApiContext.Orders
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (orderToUpdate == null)
            {
                throw new NullReferenceException();
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

            if (orderToDelete == null)
            {
                throw new NullReferenceException();
            }

            WebShopApiContext.Orders.Remove(orderToDelete);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}