using AShop.API.Models;
using AShop.API.Services.IService;

namespace AShop.API.Services.Interface
{
    public interface IUserService:IService<ApplicationUser>
    {
        Task<bool> ChangeRole(string userId, string roleName);
    }
}
