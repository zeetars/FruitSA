using FruitSA.Model;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace FruitSA.API.Models
{
    public class ProductRepository : IProductRepository
    {
        
        private readonly AppDbContext appDbContext;

        public IMapper Mapper { get; set; }

        public ProductRepository(AppDbContext appDbContext, IMapper Mapper)
        {
            this.appDbContext = appDbContext;
            this.Mapper = Mapper;
        }

        public async Task<int> GetProductCount()
        {
            return await appDbContext.Products.CountAsync();
        }

        public async Task<bool> GetProductByCode(string productCode)
        {
            var result =  await appDbContext.Products.FirstOrDefaultAsync(p => p.ProductCode == productCode);
            if (result == null) 
            {
                return false;
            }

            return true;
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

                //result = Mapper.Map(Product, result);

                await appDbContext.SaveChangesAsync();
                return result;
            }

            return null;
        }

    }
}
