using AShop.API.Models;
using AShop.API.Services.IService;
using System.Linq.Expressions;

namespace AShop.API.Services.Interface
{
    public interface ICategoryService:IService<Category>
    {

        Task<bool> Edit(int id, Category category, CancellationToken cancellationToken = default);
        Task<bool> Toggle(int id, CancellationToken cancellationToken = default);
     

    }
}
