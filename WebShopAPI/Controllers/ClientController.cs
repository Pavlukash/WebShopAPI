using System.Threading;
using System.Threading.Tasks;
using WebShop.Domain.Models;
using WebShop.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private IClientService ClientService { get; }

        public ClientController(IClientService clientService)
        {
            ClientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients(CancellationToken cancellationToken)
        {
            var result = await ClientService.GetClients(cancellationToken);
            
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await ClientService.GetById(id, cancellationToken);

            return Ok(result);
        }

        [HttpPost("GiveDiscount")]
        public async Task<IActionResult> GiveDiscount(int clientId, int discountId, CancellationToken cancellationToken)
        {
            var result = await ClientService.GiveDiscount(clientId, discountId, cancellationToken);

            return Ok(result);
        }

        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(int id,[FromBody] ClientDto clientEntity)
        {
            var result = await ClientService.Update(id, clientEntity, CancellationToken.None);
            
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await ClientService.Delete(id, CancellationToken.None);
            
            return Ok(result);
        }

        [HttpPut("Ban/{id:int}")]
        public async Task<IActionResult> BanUser(int id, CancellationToken cancellationToken)
        {
            var result = await ClientService.BanUser(id, cancellationToken);

            return Ok(result);
        }
        
        [HttpPut("Unban/{id:int}")]
        public async Task<IActionResult> UnbanUser(int id, CancellationToken cancellationToken)
        {
            var result = await ClientService.UnbanUser(id, cancellationToken);

            return Ok(result);
        }
    }
}