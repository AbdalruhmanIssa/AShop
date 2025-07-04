﻿using AShop.API.Models;
using AShop.API.Services.IService;

namespace AShop.API.Services.Interface
{
    public interface ICartService:IService<Cart>
    {
        Task<Cart> AddToCart(string UserId, int ProductId, CancellationToken cancellationToken);
         Task<IEnumerable<Cart>> GetUserCartAsync(string UserId);
        
    }
}
