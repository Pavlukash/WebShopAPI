using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebShop.Domain.Models;
using WebShop.Services.Interfaces;

namespace WebShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderService OrderService { get; }

        public OrderController(IOrderService clientService)
        {
            OrderService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders(CancellationToken cancellationToken)
        {
            var isAdmin = HttpContext.User.IsInRole("admin");
            
            var result = await OrderService.GetOrders(isAdmin, cancellationToken);

            return Ok(result);
        }

        [HttpGet("Client/{clientId:int}")]
        public async Task<IActionResult> GetClientOrders(int clientId, CancellationToken cancellationToken)
        {
            var result = await OrderService.GetClientsOrders(clientId, cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await OrderService.GetById(id, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Сreate([FromBody] OrderDto orderEntity)
        {
            var result = await OrderService.Create(orderEntity, CancellationToken.None);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderDto orderEntity)
        {
            var result = await OrderService.Update(id, orderEntity, CancellationToken.None);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await OrderService.Delete(id, CancellationToken.None);

            return Ok(result);
        }
    }
}