using FruitSA.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FruitSA.API.Models
{
    public class ProductRepository : IProductRepository
    {
        
        private readonly AppDbContext appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<int> GetProductCount()
        {
            return await appDbContext.Products.CountAsync();
        }

        public async Task<Product> AddProduct(Product Product)
        {
            
            var result = await appDbContext.Products.AddAsync(Product);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Product> DeleteProduct(int ProductId)
        {
            var result = await appDbContext.Products.FirstOrDefaultAsync(p => p.ProductId == ProductId);
            if(result != null)
            {
                appDbContext.Products.Remove(result);
                await appDbContext.SaveChangesAsync();
                return result;
            }
            return null;
        }

        public async Task<Product> GetProductById(int ProductId)
        {
            return await appDbContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == ProductId);

        }

        public async Task<IEnumerable<Product>> GetProducts(int pageNumber, int pageSize)
        {

            //return await appDbContext.Products
            //    .Include(p => p.Category)
            //    .ToListAsync();

            return await appDbContext.Products
                .Include(p => p.Category)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Product> UpdateProduct(Product Product)
        {
            var result = await appDbContext.Products.FirstOrDefaultAsync(p => p.ProductId == Product.ProductId);
            if(result != null)
            {
                result.ProductCode = Product.ProductCode;
                result.Name = Product.Name;
                result.Description = Product.Description;
                result.CategoryId = Product.CategoryId;
                result.Price = Product.Price;
                result.Image = Product.Image;

                await appDbContext.SaveChangesAsync();
                return result;
            }

            return null;
        }

    }
}
