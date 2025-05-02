using AShop.API.DTOs.Requests;
using AShop.API.DTOs.Responses;
using AShop.API.Models;
using AShop.API.Services.Interface;
using AShop.API.Services.varService;
using AShop.API.Utility;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{StaticData.SuperAdmin},{StaticData.Admin},{StaticData.Company}")]

    public class ProductsController(IProductService productService) : ControllerBase
    {
        private readonly IProductService productService= productService;
        [HttpGet("")]
        [AllowAnonymous()]
        public IActionResult GetAll([FromQuery] string? query,[FromQuery]int page, [FromQuery]int limit=10) {
            IQueryable<Product> products = productService.GetAll();
            if (query != null)
            {
                products=products.Where(p=>p.Name.Contains(query) || p.Description.Contains(query));
            }
            if (page < 1 || limit<1) { 
            page = 1;
                limit = 10;
            }
            products = products.Skip((page - 1) * limit).Take(limit);
            return Ok(products.Adapt<IEnumerable<ProductResponse>>());
        }
        [HttpGet("{id}")]
        [AllowAnonymous()]
        public IActionResult GetById([FromRoute] int id) {
        
        var product = productService.Get(e=>e.Id==id);
        return product == null? NotFound(): Ok(product.Adapt<ProductResponse>());
        }
        [HttpPost]
        public IActionResult CreateProduct([FromForm] ProductRequest productRequest)
        {
            var product = productRequest.Adapt<Product>(); 
            var createdProduct = productService.Add(product, productRequest.mainImg);

            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] ProductUpdateRequest productRequest)
        {
            await productService.UpdateProductAsync(id, productRequest);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var result = productService.Remove(id);
            return result ? NoContent() : NotFound();
        }

    }
}
