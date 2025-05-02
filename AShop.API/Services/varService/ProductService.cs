using AShop.API.Data;
using AShop.API.DTOs.Requests;
using AShop.API.Models;
using AShop.API.Services.Interface;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AShop.API.Services.varService
{
    public class ProductService : IProductService
    {
        ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Product Add(Product product, IFormFile mainImage)
        {
            if (mainImage != null && mainImage.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(mainImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    mainImage.CopyTo(stream);
                }

                product.mainImg = fileName;
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            return product;
        }



        public Product? Get(Expression<Func<Product, bool>> expression)
        {
            return _context.Products.FirstOrDefault(expression);
        }

        public IQueryable<Product> GetAll()
        {
            return _context.Products;
        }

        public bool Remove(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return false;

           
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", product.mainImg);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return true;
        }
        public async Task UpdateProductAsync(int id, ProductUpdateRequest productRequest)
        {
            var productInDb = _context.Products.AsNoTracking().FirstOrDefault(p => p.Id == id);
            var product = productRequest.Adapt<Product>();
            var file = productRequest.mainImg;

            if (productInDb != null)
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "images", productInDb.mainImg);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                    product.mainImg = fileName;
                }
                else
                {
                    product.mainImg = productInDb.mainImg;
                }

                product.Id = id;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
