using AShop.API.Models;
using System.Linq.Expressions;

namespace AShop.API.Services.Interface
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        Product Get(Expression<Func<Product, bool>> expression);
        Product Add(Product product, IFormFile mainImage);
        bool Remove(int id);
    }
}
