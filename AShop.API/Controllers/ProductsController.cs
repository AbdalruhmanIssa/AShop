using AShop.API.DTOs.Requests;
using AShop.API.DTOs.Responses;
using AShop.API.Models;
using AShop.API.Services.Interface;
using AShop.API.Services.varService;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        private readonly IProductService productService= productService;
        [HttpGet("")]
        public IActionResult GetAll() {
            var products = productService.GetAll();
            return Ok(products.Adapt<IEnumerable<ProductResponse>>());
        }
        [HttpGet("{id}")]
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
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var result = productService.Remove(id);
            return result ? NoContent() : NotFound();
        }

    }
}
