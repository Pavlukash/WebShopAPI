using System.Threading;
using System.Threading.Tasks;
using WebShop.Common.Responses;
using WebShop.Domain.Models;

namespace WebShop.Services.Interfaces
{
    public interface IJwtService
    {
        Task<LoginResponse> Login(string username, string password, CancellationToken cancellationToken);
        Task<ClientDto> Register(ClientDto newClientEntity, CancellationToken cancellationToken);
    }
}