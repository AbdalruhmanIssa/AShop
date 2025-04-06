using AShop.API.Data;
using AShop.API.DTOs.Requests;
using AShop.API.DTOs.Responses;
using AShop.API.Models;
using AShop.API.Services.Interface;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService categoryService) : ControllerBase
    {
        private readonly ICategoryService categoryService= categoryService;



        [HttpGet("")]
        public IActionResult GetAll()
        {
            var categories = categoryService.GetAll();
            return Ok(categories.Adapt<IEnumerable<CategoryResponse>>());
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var category = categoryService.Get(e=>e.Id==id);
            return category == null ? NotFound() : Ok(category.Adapt<CategoryResponse>());
        }

        [HttpPost("")]
        public IActionResult Create([FromBody] CategoryRequest category)
        {
            var cat=categoryService.Add(category.Adapt<Category>());
            return CreatedAtAction(nameof(GetById),new {cat.Id},cat);
        }
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id,[FromBody] CategoryRequest category) {
            var cat=categoryService.Edit(id,category.Adapt<Category>());
            if(!cat) return NotFound();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var category = categoryService.Remove(id);
            if (!category)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
