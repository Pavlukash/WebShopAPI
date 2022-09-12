using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebShop.Domain.Models;

namespace WebShop.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts(CancellationToken cancellationToken);
        Task<ProductDto> GetById(int id, CancellationToken cancellationToken);
        Task<List<ProductDto>> GetClientsProductList(CancellationToken cancellationToken);
        Task<bool> AddToProductList(int id, CancellationToken cancellationToken);
        Task<bool> RemoveFromProductList(int id, CancellationToken cancellationToken);
        Task<ProductDto> Create(ProductDto productEntity, CancellationToken cancellationToken);
        Task<bool> Update(int id, ProductDto productEntity, CancellationToken cancellationToken);
        Task<bool> Delete(int id, CancellationToken cancellationToken);
    }
}