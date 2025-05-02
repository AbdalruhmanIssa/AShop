using AShop.API.DTOs.Requests;
using AShop.API.Models;
using AShop.API.Services.Interface;
using Azure.Core;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AShop.API.Services.varService
{
    public class AccountService(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager):IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly SignInManager<ApplicationUser>signInManager=signInManager;

        public async Task<IdentityResult> ChangePasswordAsync(ClaimsPrincipal userPrincipal,ChangePasswordRequest changePasswordRequest)
        {
            var user = await userManager.GetUserAsync(userPrincipal);

            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            var result = await userManager.ChangePasswordAsync(user, changePasswordRequest.OldPassword, changePasswordRequest.NewPassword);
            return result;
        }

        public async Task<SignInResult> LoginAsync(LoginRequest loginRequest)
        {
            var AppUser = await userManager.FindByEmailAsync(loginRequest.Email);//ApplecationUser?
            if (AppUser == null)  return SignInResult.Failed; 

            var Pass = await userManager.CheckPasswordAsync(AppUser, loginRequest.Password);//Boolean
            if(!Pass) return SignInResult.Failed;
           
            await signInManager.SignInAsync(AppUser, loginRequest.RememberMe);//cokies


            return SignInResult.Success;

        }
        public async Task<IdentityResult> RegisterAsync(RegisterRequest registerRequest)
        {
            var applicationUser = registerRequest.Adapt<ApplicationUser>();

            var result = await userManager.CreateAsync(applicationUser, registerRequest.Password);
            return result;

        }

    }
}
