using WebShop.Domain.Entities;
using WebShop.Domain.Models;

namespace WebShop.Services.Mappers
{
    public static class OrderMapper
    {
        public static OrderDto ToDto(this OrderEntity entity)
        {
            var result = new OrderDto
            {
                Id = entity.Id,
                ClientId = entity.ClientId,
                TotalPrice = entity.TotalPrice
            };
            
            return result;
        }
    }
}