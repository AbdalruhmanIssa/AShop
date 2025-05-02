using AShop.API.Data;
using AShop.API.Models;
using AShop.API.Services.Interface;
using AShop.API.Services.IService;

namespace AShop.API.Services.varService
{
    public class CartService : Service<Cart>, ICartService
    {
        private readonly ApplicationDbContext _context;
        public CartService(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
