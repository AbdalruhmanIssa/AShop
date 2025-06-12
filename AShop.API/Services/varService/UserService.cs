using AShop.API.Data;
using AShop.API.Models;
using AShop.API.Services.Interface;
using AShop.API.Services.IService;
using Microsoft.AspNetCore.Identity;

namespace AShop.API.Services.varService
{
    public class UserService : Service<ApplicationUser>, IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(ApplicationDbContext context,UserManager<ApplicationUser> userManager) : base(context)
        {
            this._context = context;
            this._userManager = userManager;
        }

        public async Task<bool> ChangeRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is not null)
            {
                var oldRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, oldRoles);

                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }
        public async Task<bool?> LockUnLock(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null) return null;

            var isLockedNow = user.LockoutEnabled && user.LockoutEnd > DateTime.Now;

            if (isLockedNow)
            {
                // Remove lock
                user.LockoutEnabled = false;
                user.LockoutEnd = null;
            }
            else
            {
                // Apply lock
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTime.Now.AddMinutes(1);
            }

            await _userManager.UpdateAsync(user);

            return !isLockedNow;
        }



    }
}
