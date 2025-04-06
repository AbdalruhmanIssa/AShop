using AShop.API.Models;
using System.Linq.Expressions;

namespace AShop.API.Services.Interface
{
    public interface IBrandService
    {
        IEnumerable<Brand> GetAll();
        Brand Get(Expression<Func<Brand, bool>> expression);
        Brand Add(Brand brand);
        bool Edit(int id, Brand brand);
        bool Remove(int id);
    }
}
