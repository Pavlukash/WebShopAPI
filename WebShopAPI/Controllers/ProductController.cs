using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebShop.Domain.Models;
using WebShop.Services.Interfaces;

namespace WebShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService ProductService { get; }

        public ProductController(IProductService productService)
        {
            ProductService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
        {
            var result = await ProductService.GetProducts(cancellationToken);
            
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var result = await ProductService.Get(id, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Сreate([FromBody] ProductDto productEntity)
        {
            var result = await ProductService.Create(productEntity, CancellationToken.None);
            
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id,[FromBody] ProductDto productEntity)
        {
            var result = await ProductService.Update(id, productEntity, CancellationToken.None);
            
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await ProductService.Delete(id, CancellationToken.None);
            
            return Ok(result);
        }
    }
}