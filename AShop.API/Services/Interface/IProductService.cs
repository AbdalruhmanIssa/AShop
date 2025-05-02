using AShop.API.DTOs.Requests;
using AShop.API.Models;
using System.Linq.Expressions;

namespace AShop.API.Services.Interface
{
    public interface IProductService
    {
        IQueryable<Product> GetAll();
        Product Get(Expression<Func<Product, bool>> expression);
        Product Add(Product product, IFormFile mainImage);
        bool Remove(int id);
        Task UpdateProductAsync(int id, ProductUpdateRequest productRequest);
    }
}
