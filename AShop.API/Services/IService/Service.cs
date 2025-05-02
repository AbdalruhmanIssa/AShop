using AShop.API.Data;
using AShop.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AShop.API.Services.IService
{
    public class Service<T>(ApplicationDbContext context) : IService<T> where T : class
    {
        private readonly ApplicationDbContext context=context;
        private readonly DbSet<T> db=context.Set<T>();//eyes here
        public async Task<T> Add(T entity, CancellationToken cancellationToken = default)
        {
            await context.AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<T?> Get(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>?[] include = null
            , bool istracked = true)
        {
            var entity=await GetAllAsync(expression,include,istracked);
            return entity.FirstOrDefault();

        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>?[] include = null
            , bool istracked = true)
        {
            IQueryable<T> query=db;
            if (include != null)
            {
                foreach (var item in include)
                {
                    query = query.Include(item);
                }
            }
            if (expression is not null)
            {
                query=query.Where(expression);
            }
            if (!istracked)
            {
                query=query.AsNoTracking();
            }
            return await query.ToListAsync();
        }

        public async Task<bool> Remove(int id,CancellationToken cancellationToken)
        {
            T? entityInDb = db.Find(id);
            if (entityInDb == null) return false;
            db.Remove(entityInDb);
            await   context.SaveChangesAsync(cancellationToken);
            return true;
        }

        
    }
}
