using AShop.API.Data;
using AShop.API.Models;
using AShop.API.Services.Interface;
using AShop.API.Services.IService;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AShop.API.Services.varService
{
    public class CategoryService : Service<Category>,ICategoryService
    {
        ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context):base(context) 
        {
            _context = context;
        }

       



        public async Task<bool> Edit(int id, Category category,CancellationToken cancellationToken=default)
        {
            Category? categoryInDb = _context.Categories.Find(id);
            if (categoryInDb == null) return false;
            categoryInDb.Name = category.Name;
            categoryInDb.Description = category.Description;
           await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        public async Task<bool> Toggle(int id, CancellationToken cancellationToken = default)
        {
            Category? categoryInDb = _context.Categories.Find(id);
            if (categoryInDb == null) return false;
            categoryInDb.Status = !categoryInDb.Status;
            await _context.SaveChangesAsync(cancellationToken);
            return true;

        }


     



    }
}
