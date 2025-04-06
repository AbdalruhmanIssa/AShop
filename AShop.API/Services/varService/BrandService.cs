using AShop.API.Data;
using AShop.API.Models;
using AShop.API.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AShop.API.Services.varService
{
    public class BrandService : IBrandService
    {
        ApplicationDbContext _context;

        public BrandService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Brand Add(Brand brand)
        {
            _context.Brands.Add(brand);
            _context.SaveChanges();
            return brand;
        }

        public bool Edit(int id, Brand brand)
        {
            Brand? brandInDb = _context.Brands.AsNoTracking().FirstOrDefault(i => i.Id == id);
            if (brandInDb == null) return false;
           brandInDb.Id = id;
            _context.Brands.Update(brand);
            _context.SaveChanges();
            return true;
        }

        public Brand? Get(Expression<Func<Brand, bool>> expression)
        {
            return _context.Brands.FirstOrDefault(expression);
        }

        public IEnumerable<Brand> GetAll()
        {
            return _context.Brands.ToList();
        }

        public bool Remove(int id)
        {
            Brand? brand = _context.Brands.Find(id);
            if (brand == null) return false;

            _context.Brands.Remove(brand);
            _context.SaveChanges();
            return true;
        }

        
    }
}
