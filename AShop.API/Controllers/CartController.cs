using AShop.API.DTOs.Responses;
using AShop.API.Models;
using AShop.API.Services.Interface;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        public async Task<IActionResult> AddToCart([FromRoute] int ProductId, CancellationToken cancellationToken)
        {
            var appUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var result = await cartService.AddToCart(appUser, ProductId, cancellationToken);

            return Ok();
        }
        [HttpGet("")]
        public async Task<IActionResult> GetUserCartAsync()
        {
            var appUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cartItems = await cartService.GetUserCartAsync(appUser);

            var cartResponse = cartItems.Select(e => e.Product).Adapt<IEnumerable<cartResponse>>();
            var totalPrice = cartItems.Sum(e => e.Product.Price * e.Count);

            return Ok(new { cartResponse, totalPrice });
        }

    }
}
