using AShop.API.DTOs.Requests;
using AShop.API.DTOs.Responses;
using AShop.API.Models;
using AShop.API.Services.Interface;
using AShop.API.Services.varService;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController(IBrandService brandService) : ControllerBase
    {
        private readonly IBrandService brandService = brandService;
        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var brand = await brandService.GetAllAsync();
            return Ok(brand.Adapt<IEnumerable<BrandResponse>>());

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var brand =await brandService.Get(e => e.Id == id);
            return brand == null ? NotFound() : Ok(brand.Adapt<BrandResponse>());
        }
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] BrandRequest brand,CancellationToken cancellationToken)
        {

            var b =await brandService.Add(brand.Adapt<Brand>(),cancellationToken);
            return CreatedAtAction(nameof(GetById), new { b.Id }, b);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] BrandRequest brand)
        {
            var brand1 = await brandService.Edit(id, brand.Adapt<Brand>());
            if (!brand1) return NotFound();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id,CancellationToken cancellationToken)
        {
            var brand = await brandService.Remove(id,cancellationToken);
            if (!brand)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
