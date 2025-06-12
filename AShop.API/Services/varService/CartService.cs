using AShop.API.Data;
using AShop.API.Models;
using AShop.API.Services.Interface;
using AShop.API.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace AShop.API.Services.varService
{
    public class CartService : Service<Cart>, ICartService
    {
        private readonly ApplicationDbContext _context;
        public CartService(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }
        public async Task<Cart> AddToCart(string UserId, int ProductId, CancellationToken cancellationToken)
        {
            var exisitingCartItems = await _context.Carts.FirstOrDefaultAsync(e => e.ApplicationUserId == UserId && e.ProductId == ProductId);

            if (exisitingCartItems is not null)
            {
                exisitingCartItems.Count += 1;
                await _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                exisitingCartItems = new Cart
                {
                    ApplicationUserId = UserId,
                    ProductId = ProductId,
                    Count = 1
                };

                await _context.Carts.AddAsync(exisitingCartItems, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return exisitingCartItems;
        }
        public async Task<IEnumerable<Cart>> GetUserCartAsync(string UserId)
        {
            return await GetAllAsync(e => e.ApplicationUserId == UserId, include: [c => c.Product]);
        }

    }
}
