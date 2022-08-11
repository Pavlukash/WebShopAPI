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
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var result = await ClientService.Get(id, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Сreate([FromBody] ClientDto clientEntity)
        {
            var result = await ClientService.Create(clientEntity, CancellationToken.None);
            
            return Ok(result);
        }

        [HttpPut("{id:int}")]
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
    }
}