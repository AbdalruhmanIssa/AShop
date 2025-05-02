using AShop.API.Models;
using AShop.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController(ICartService cartService, UserManager<ApplicationUser> userManager) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly ICartService cartService = cartService;
        [HttpPost("{ProductId}")]
        public async Task<IActionResult> AddToCart([FromRoute] int ProductId, [FromQuery] int Count)
        {
            var user = userManager.GetUserId(User);
            var cart = new Cart() { ProductId = ProductId, Count = Count, ApplicationUserId = user };
            await cartService.Add(cart);
            return Ok(cart);
        }
    }
}
