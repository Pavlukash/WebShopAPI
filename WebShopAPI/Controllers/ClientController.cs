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
            var isAdmin = HttpContext.User.IsInRole("admin");
            
            var result = await ClientService.GetClients(isAdmin, cancellationToken);
            
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var isAdmin = HttpContext.User.IsInRole("admin");
            
            var result = await ClientService.GetById(id, isAdmin, cancellationToken);

            return Ok(result);
        }

        [HttpPut("update{id:int}")]
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

        [HttpPut("ban{id:int}")]
        public async Task<IActionResult> BanUser(int id, CancellationToken cancellationToken)
        {
            var isAdmin = HttpContext.User.IsInRole("admin");
            
            var result = await ClientService.BanUser(id, isAdmin, cancellationToken);

            return Ok(result);
        }
        
        [HttpPut("unban{id:int}")]
        public async Task<IActionResult> UnbanUser(int id, CancellationToken cancellationToken)
        {
            var isAdmin = HttpContext.User.IsInRole("admin");
            
            var result = await ClientService.UnbanUser(id, isAdmin, cancellationToken);

            return Ok(result);
        }
    }
}