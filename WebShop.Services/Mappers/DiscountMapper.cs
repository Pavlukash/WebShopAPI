using WebShop.Domain.Entities;
using WebShop.Domain.Models;

namespace WebShop.Services.Mappers
{
    public static class DiscountMapper
    {
        public static DiscountDto ToDto(this DiscountEntity entity)
        {
            var result = new DiscountDto
            {
                ProductId = entity.Id,
                Discount = entity.Discount
            };

            return result;
        }
    }
}