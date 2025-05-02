using AShop.API.Data;
using AShop.API.Models;
using AShop.API.Services.Interface;
using AShop.API.Services.IService;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AShop.API.Services.varService
{
    public class BrandService : Service<Brand>,IBrandService
    {
        ApplicationDbContext _context;

        public BrandService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> Edit(int id, Brand brand, CancellationToken cancellationToken = default)
        {
            Brand? brandInDb = _context.Brands.Find(id);
            if (brandInDb == null) return false;
            brandInDb.Name = brand.Name;
            brandInDb.Description = brand.Description;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        
    }
}
