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
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
            var applicationUser = registerRequest.Adapt<ApplicationUser>();
            var result = await userManager.CreateAsync(applicationUser, registerRequest.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(applicationUser, StaticData.Customer);
          
                var token = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                var emailConfirmUrl = Url.Action(nameof(ConfirmEmail), "Account", new { token, userId = applicationUser.Id },
                    protocol: Request.Scheme, // http or https
                    host: Request.Host.Value
                );

                await emailSender.SendEmailAsync(applicationUser.Email, "Confirm Email",
                    $"<h1> Hello {applicationUser.UserName} </h1> <p> t-tshop , new account </p>" +
                    $"<a href='{emailConfirmUrl}'>click here </a>");
                return NoContent();
            }
            return BadRequest(result.Errors);
        }
       [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user is not null)
            {
                var result = await userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    return Ok(new { message = " email confirmed " });
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }

            return NotFound();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var token = await accountService.LoginAsync(loginRequest);

            if (token == null)
                return BadRequest(new { message = "Invalid email or password" });

            return Ok(new { token });
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
