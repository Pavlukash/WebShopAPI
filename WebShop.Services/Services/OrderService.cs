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
using WebShop.Services.Extentions;

namespace WebShop.Services.Services
{
    public class OrderService : IOrderService
    {
        private WebShopApiContext WebShopApiContext { get; }
        private ICurrentUserService CurrentUserService { get; }

        public OrderService(WebShopApiContext context, ICurrentUserService currentUserService)
        {
            WebShopApiContext = context;
            CurrentUserService = currentUserService;
        }

        public async Task<IEnumerable<OrderDto>> GetOrders(CancellationToken cancellationToken)
        {
            bool isAdmin = await CurrentUserService.CheckAdmin(null, cancellationToken);
            
            if (isAdmin == false)
            {
                throw new UnauthorizedAccessException("You are not an admin");
            }
            
            var orders = await WebShopApiContext.Orders
                .AsNoTracking()
                .Select(x => x.ToDto())
                .ToListAsync(cancellationToken);
            
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
                .FirstOrNotFoundAsync(cancellationToken);

            var result = order.ToDto();

            return result;
        }

        public async Task<OrderDto> Create(OrderDto newOrderEntity, CancellationToken cancellationToken)
        {
            var newEntity = new OrderEntity()
            {
                ClientId = 1,
                TotalPrice = await CalculateTotalPrice(cancellationToken)
            };

            WebShopApiContext.Orders.Add(newEntity);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            var result = newEntity.ToDto();

            return result;
        }

        private async Task<decimal?> CalculateTotalPrice(CancellationToken cancellationToken)
        {
            var client = await WebShopApiContext.Clients
                .AsNoTracking()
                .Include(x => x.ClientsProductsEntities)
                .ThenInclude(x =>x.ProductEntity)
                .Where(x => x.Id == 1)
                .FirstOrNotFoundAsync(cancellationToken);

            if (client.ClientsProductsEntities == null)
            {
                throw new NullReferenceException();
            }
            
            decimal? result = 0;
            
            foreach (var clientsProduct in client.ClientsProductsEntities)
            {
                var discount = await WebShopApiContext.ClientsDiscounts
                    .Include(x => x.DiscountEntity)
                    .Where(x => x.ClientId == 1)
                    .Where(x => x.DiscountEntity.ProductId == clientsProduct.ProductId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (discount == null)
                {
                    result += clientsProduct.ProductEntity.Price;
                    continue;
                }

                result += clientsProduct.ProductEntity.Price * (1 - discount.DiscountEntity.Discount / 100);
            }
            
            if (result == null)
            {
                throw new NullReferenceException();
            }

            return result;
        }

        public async Task<bool> Delete(int id, CancellationToken cancellationToken)
        {
            var orderToDelete = await WebShopApiContext.Orders
                .Where(x => x.Id == id)
                .FirstOrNotFoundAsync(cancellationToken);

            WebShopApiContext.Orders.Remove(orderToDelete);
            await WebShopApiContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}