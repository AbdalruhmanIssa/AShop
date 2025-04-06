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
    public class BrandsController(IBrandService brandService) : ControllerBase
    {
        private readonly IBrandService brandService = brandService;
        [HttpGet("")]
        public IActionResult GetAll()
        {
            var brand = brandService.GetAll();
            return Ok(brand.Adapt<IEnumerable<BrandResponse>>());

        }
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var brand = brandService.Get(e => e.Id == id);
            return brand == null ? NotFound() : Ok(brand.Adapt<BrandResponse>());
        }
        [HttpPost("")]
        public IActionResult Create([FromBody] BrandRequest brand)
        {

            var b = brandService.Add(brand.Adapt<Brand>());
            return CreatedAtAction(nameof(GetById), new { b.Id }, b);
        }
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] BrandRequest brand)
        {
            var brand1 = brandService.Edit(id, brand.Adapt<Brand>());
            if (!brand1) return NotFound();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var brand = brandService.Remove(id);
            if (!brand)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
