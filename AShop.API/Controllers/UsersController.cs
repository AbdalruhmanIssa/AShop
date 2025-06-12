using AShop.API.DTOs.Requests;
using AShop.API.Models;
using AShop.API.Services.Interface;
using AShop.API.Services.varService;
using AShop.API.Utility;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =$"{StaticData.SuperAdmin}")]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService userService =userService;
        [HttpGet("")]
        public async Task<IActionResult> GetAll() {
            var users= await userService.GetAllAsync();    
            return Ok(users.Adapt<IEnumerable <UserDTO>>());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        { 
            var user=await userService.Get(u=> u.Id == id);
            return Ok(user.Adapt<UserDTO>());
        }
        [HttpPut("{userId}")]
        public async Task<IActionResult> ChangeRole([FromRoute] string userId, [FromQuery] string newRoleName)
        {
            var result = await userService.ChangeRole(userId, newRoleName);
            return Ok(result);
        }
        [HttpPost("lock-unlock")]
        public async Task<IActionResult> LockUnLock([FromBody] string userId)

        {
            var result = await userService.LockUnLock(userId);

            if (result == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }


    }
}
