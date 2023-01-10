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
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await ProductService.GetById(id, cancellationToken);

            return Ok(result);
        }

        [HttpGet("GetProductList")]
        public async Task<IActionResult> GetClientsProductList(CancellationToken cancellationToken)
        {
            var result = await ProductService.GetClientsProductList(cancellationToken);

            return Ok(result);
        }

        [HttpPost("AddToCart/{id:int}")]
        public async Task<IActionResult> AddToProductList(int id, CancellationToken cancellationToken)
        {
            var result = await ProductService.AddToProductList(id, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("RemoveFromCart/{id:int}")]
        public async Task<IActionResult> RemoveFromProductList(int id, CancellationToken cancellationToken)
        {
            var result = await ProductService.RemoveFromProductList(id, cancellationToken);

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