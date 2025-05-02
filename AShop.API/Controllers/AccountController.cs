using AShop.API.DTOs.Requests;
using AShop.API.Models;
using AShop.API.Services.Interface;
using AShop.API.Services.varService;
using AShop.API.Utility;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace AShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager
        ,IAccountService accountService,
        IEmailSender emailSender,
        RoleManager<IdentityRole> roleManager) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly SignInManager<ApplicationUser> signInManager = signInManager;
        private readonly IAccountService accountService= accountService;
        private readonly IEmailSender emailSender = emailSender;
        private readonly RoleManager<IdentityRole> roleManager = roleManager;


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var UA = registerRequest.Adapt<ApplicationUser>();
            var result=await userManager.CreateAsync(UA,registerRequest.Password);
            if (roleManager.Roles.IsNullOrEmpty()) {
                await roleManager.CreateAsync(new("SuperAdmin"));
                await roleManager.CreateAsync(new("Admin"));
                await roleManager.CreateAsync(new("Customer"));
                await roleManager.CreateAsync(new("Company"));
            }

            if (result.Succeeded)
            {
               await emailSender.SendEmailAsync(UA.Email, "flfllf", "<h1></h1>");
              await  userManager.AddToRoleAsync(UA,StaticData.Customer);
                await signInManager.SignInAsync(UA,false);
                return NoContent();
            }

            return BadRequest(result.Errors);
        }




        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var x=await accountService.LoginAsync(loginRequest);
                if (x.Succeeded)
                {
                    return NoContent();//no content to return 
                }

            
            return BadRequest(new { message = "invalid email or password" }); //if all fails
        }




        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();//delete cokies too
            return NoContent();
        }

        [Authorize]// so we make sure nobody do the certin process without having an account
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordRequest changePassword)
        {
            
            var appuser = await accountService.ChangePasswordAsync(User,changePassword);
        if (appuser.Succeeded)
                return NoContent();
            return BadRequest(appuser.Errors);


        }
    }
}
