using AShop.API.Models;
using System.Linq.Expressions;

namespace AShop.API.Services.IService
{
    public interface IService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>>?expression=null,Expression< Func<T,object>>?[] include=null, bool istracked = true);
      Task<T> Get(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>?[] include = null
            , bool istracked = true);
        Task<T> Add(T category, CancellationToken cancellationToken = default);
         Task<bool> Remove(int id, CancellationToken cancellationToken);
    }
}
