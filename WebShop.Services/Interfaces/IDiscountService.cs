using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebShop.Domain.Models;

namespace WebShop.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<IEnumerable<DiscountDto>> GetDiscounts(bool isAdmin, CancellationToken cancellationToken);
        Task<IEnumerable<DiscountDto>> GetClientsDiscounts(int id, CancellationToken cancellationToken);
        Task<DiscountDto> GetById(int id, bool isAdmin, CancellationToken cancellationToken);
        Task<DiscountDto> Create(DiscountDto newDiscountEntity, bool isAdmin, CancellationToken cancellationToken);
        Task<bool> Update(int id, DiscountDto discountEntity, bool isAdmin, CancellationToken cancellationToken);
        Task<bool> Delete(int id, bool isAdmin, CancellationToken cancellationToken);
    }
}