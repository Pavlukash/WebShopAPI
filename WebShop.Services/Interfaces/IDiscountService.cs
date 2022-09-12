using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebShop.Domain.Models;

namespace WebShop.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<IEnumerable<DiscountDto>> GetDiscounts(CancellationToken cancellationToken);
        Task<IEnumerable<DiscountDto>> GetClientsDiscounts(int id, CancellationToken cancellationToken);
        Task<DiscountDto> GetById(int id, CancellationToken cancellationToken);
        Task<DiscountDto> Create(DiscountDto newDiscountEntity, CancellationToken cancellationToken);
        Task<bool> Update(int id, DiscountDto discountEntity, CancellationToken cancellationToken);
        Task<bool> Delete(int id, CancellationToken cancellationToken);
    }
}