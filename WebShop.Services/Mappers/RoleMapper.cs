using WebShop.Domain.Entities;
using WebShop.Domain.Models;

namespace WebShop.Services.Mappers
{
    public static class RoleMapper
    {
        public static RoleDto ToDto(this RoleEntity entity)
        {
            var result = new RoleDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                CanApplyDiscounts = entity.CanApplyDiscounts,
                CanEditProducts = entity.CanEditProducts,
                CanEditRoles = entity.CanEditRoles,
                CanBanUsers = entity.CanBanUsers
            };

            return result;
        }
    }
}