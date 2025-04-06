using AShop.API.Data;
using AShop.API.Models;
using AShop.API.Services.Interface;
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

        public IEnumerable<Product> GetAll()
        {
           return _context.Products.ToList();
        }

        public bool Remove(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return false;

            // Delete image file if exists
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "images", product.mainImg);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return true;
        }
    }
}
