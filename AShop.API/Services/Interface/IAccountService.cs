using AShop.API.DTOs.Requests;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AShop.API.Services.Interface
{
    public interface IAccountService
    {
       
        Task<string?> LoginAsync(LoginRequest loginRequest);
        Task<IdentityResult> ChangePasswordAsync(ClaimsPrincipal user, ChangePasswordRequest changePasswordRequest);
    }
}
