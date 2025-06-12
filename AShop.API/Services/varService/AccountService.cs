using AShop.API.DTOs.Requests;
using AShop.API.Models;
using AShop.API.Services.Interface;
using AShop.API.Utility;
using Azure.Core;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AShop.API.Services.varService
{
    public class AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager
        , IEmailSender emailSender) : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly IEmailSender emailSender = emailSender;
        private readonly SignInManager<ApplicationUser> signInManager = signInManager;

        public async Task<IdentityResult> ChangePasswordAsync(ClaimsPrincipal userPrincipal,
            ChangePasswordRequest changePasswordRequest)
        {
            var user = await userManager.GetUserAsync(userPrincipal);

            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            var result = await userManager.ChangePasswordAsync(user, changePasswordRequest.OldPassword, 
                changePasswordRequest.NewPassword);
            return result;
        }

        public async Task<string?> LoginAsync(LoginRequest loginRequest)
        {
            var appUser = await userManager.FindByEmailAsync(loginRequest.Email);
            if (appUser == null) return null;

            var isPasswordValid = await signInManager.PasswordSignInAsync(appUser, loginRequest.Password, loginRequest.RememberMe, false);
            if (isPasswordValid.IsLockedOut ||isPasswordValid.IsNotAllowed) return null;

            // Create claims
            List<Claim> claims = new();

            claims.Add(new(ClaimTypes.NameIdentifier, appUser.Id));
            claims.Add(new(ClaimTypes.Name, appUser.UserName));
    

            var userRoles = await userManager.GetRolesAsync(appUser);
            foreach (var role in userRoles)
            {
                claims.Add(new(ClaimTypes.Role, role));
            }

            // Generate JWT token
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes("Z4HpB0pV8BIDdAcNfKY0iweTDPS1Tku7"));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
