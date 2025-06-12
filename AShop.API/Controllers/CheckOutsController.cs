using AShop.API.Services.Interface;
using AShop.API.Services.varService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace AShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckOutsController(ICartService cartService) : ControllerBase
    {
        private readonly ICartService cartService= cartService;
        [HttpGet("Pay")]
        public async Task<IActionResult> Pay()
        {
            var appUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var carts = await cartService.GetAllAsync(e => e.ApplicationUserId == appUser, [e=>e.Product]);

            if (carts is not null)
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success",
                    CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
                };
                foreach (var item in carts)
                {
                    options.LineItems.Add(
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency = "USD",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = item.Product.Name,
                                    Description = item.Product.Description,
                                },
                                UnitAmount = (long)item.Product.Price,
                            },
                            Quantity = item.Count,
                        });
                }

                var service = new SessionService();
                var session = service.Create(options);
                return Ok(new { session.Url});
            }
            else { return NotFound(); }
        }
        [HttpGet("")]
        public async Task<IActionResult> Success()
        {
            return Ok();
        }
    }
}
