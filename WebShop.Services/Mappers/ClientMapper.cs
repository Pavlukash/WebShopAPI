using WebShop.Domain.Entities;
using WebShop.Domain.Models;

namespace WebShop.Services.Mappers
{
    public static class ClientMapper
    {
        public static ClientDto ToDto(this ClientEntity entity)
        {
            var result = new ClientDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                Password = entity.Password,
                PhoneNumber = entity.PhoneNumber
            };
            
            return result;
        }
    }
}