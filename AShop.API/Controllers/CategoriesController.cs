using AShop.API.Data;
using AShop.API.DTOs.Requests;
using AShop.API.DTOs.Responses;
using AShop.API.Models;
using AShop.API.Services.Interface;
using AShop.API.Utility;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService categoryService) : ControllerBase
    {
        private readonly ICategoryService categoryService = categoryService;



        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await categoryService.GetAllAsync();
            return Ok(categories.Adapt<IEnumerable<CategoryResponse>>());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var category =await categoryService.Get(e => e.Id == id);
            return category == null ? NotFound() : Ok(category.Adapt<CategoryResponse>());
        }

        [HttpPost("")]
        [Authorize(Roles = $"{StaticData.SuperAdmin},{StaticData.Admin},{StaticData.Company}")]
        public async Task<IActionResult> Create([FromBody] CategoryRequest category,
            CancellationToken cancellationToken)
        {
            var cat=await categoryService.Add(category.Adapt<Category>(),cancellationToken);
            return CreatedAtAction(nameof(GetById),new {cat.Id},cat);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{StaticData.SuperAdmin},{StaticData.Admin},{StaticData.Company}")]
        public async Task<IActionResult> Update([FromRoute] int id,[FromBody] CategoryRequest category) {
            var cat=await categoryService.Edit(id,category.Adapt<Category>());
            if(!cat) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{StaticData.SuperAdmin},{StaticData.Admin},{StaticData.Company}")]

        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var category =await categoryService.Remove(id,  cancellationToken);
            if (!category)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
