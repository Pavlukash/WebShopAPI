using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebShop.Domain.Models;
using WebShop.Services.Interfaces;

namespace WebShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private IDiscountService DiscountService { get; }

        public DiscountController(IDiscountService discountService)
        {
            DiscountService = discountService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetDiscounts(CancellationToken cancellationToken)
        {
            var isAdmin = HttpContext.User.IsInRole("admin");
            
            var result = await DiscountService.GetDiscounts(isAdmin, cancellationToken);

            return Ok(result);
        }
        
        [HttpGet("Client/{clientId:int}")]
        public async Task<IActionResult> GetClientOrders(int clientId, CancellationToken cancellationToken)
        {
            var result = await DiscountService.GetClientsDiscounts(clientId, cancellationToken);

            return Ok(result);
        }
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var isAdmin = HttpContext.User.IsInRole("admin");
            
            var result = await DiscountService.GetById(id, isAdmin, cancellationToken);

            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> Сreate([FromBody] DiscountDto discountEntity)
        {
            var isAdmin = HttpContext.User.IsInRole("admin");
            
            var result = await DiscountService.Create(discountEntity, isAdmin, CancellationToken.None);

            return Ok(result);
        }
        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] DiscountDto discountEntity)
        {
            var isAdmin = HttpContext.User.IsInRole("admin");
            
            var result = await DiscountService.Update(id, discountEntity, isAdmin, CancellationToken.None);

            return Ok(result);
        }
        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isAdmin = HttpContext.User.IsInRole("admin");
            
            var result = await DiscountService.Delete(id, isAdmin, CancellationToken.None);

            return Ok(result);
        }
    }
}