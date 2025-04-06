using AShop.API.Models;
using System.Linq.Expressions;

namespace AShop.API.Services.Interface
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll();
        Category Get(Expression <Func<Category,bool>> expression);
        Category Add (Category category);
        bool Edit (int id,Category category);
        bool Remove (int id);

    }
}
