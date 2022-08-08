using WebShop.Domain.Entities;
using WebShop.Domain.Models;

namespace WebShop.Services.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToDto(this ProductEntity entity)
        {
            var result = new ProductDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price
            };
            
            return result;
        }
    }
}