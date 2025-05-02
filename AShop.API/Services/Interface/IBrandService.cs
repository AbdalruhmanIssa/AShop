using AShop.API.Models;
using AShop.API.Services.IService;
using System.Linq.Expressions;

namespace AShop.API.Services.Interface
{
    public interface IBrandService:IService<Brand>
    {
        Task<bool> Edit(int id, Brand brand, CancellationToken cancellationToken = default);
       
    }
}
