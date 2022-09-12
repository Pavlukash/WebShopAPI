using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebShop.Domain.Models;
using WebShop.Services.Interfaces;

namespace WebShopAPI.Controllers
{
    public class JwtController : ControllerBase
    {
        private IJwtService JwtService { get; }
        
        public JwtController(IJwtService jwtService)
        {
            JwtService = jwtService;
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login(string email, string password, CancellationToken cancellationToken)
        {
            var result = await JwtService.Login(email, password, cancellationToken);

            return Ok(result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]ClientDto newClientEntity, CancellationToken cancellationToken)
        {
            var result = await JwtService.Register(newClientEntity, cancellationToken);

            return Ok(result);
        }
    }
}