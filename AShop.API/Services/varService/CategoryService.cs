using AShop.API.Data;
using AShop.API.Models;
using AShop.API.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AShop.API.Services.varService
{
    public class CategoryService : ICategoryService
    {
        ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Category Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }



        public bool Edit(int id, Category category)
        {
            Category? categoryInDb = _context.Categories.AsNoTracking().FirstOrDefault(i=>i.Id==id);
            if (categoryInDb == null) return false;
            categoryInDb.Id = id;
            _context.Categories.Update(category);
            _context.SaveChanges();
            return true;
        }


        public Category? Get(Expression<Func<Category, bool>> expression)
        {
            return _context.Categories.FirstOrDefault(expression) ;
            
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        public bool Remove(int id){
            Category? categoryInDb = _context.Categories.Find(id);
            if (categoryInDb == null) return false;

            _context.Categories.Remove(categoryInDb);
            _context.SaveChanges();
            return true;
        }
    }
}
