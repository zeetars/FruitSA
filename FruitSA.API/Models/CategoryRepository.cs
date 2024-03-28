using FruitSA.Model;
using Microsoft.EntityFrameworkCore;

namespace FruitSA.API.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext appDbContext;

        public CategoryRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<Category> AddCategory(Category Category)
        {
            var result = await appDbContext.Categories.AddAsync(Category);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        //public async Task<Category> DeleteCategory(int CategoryId)
        //{
        //    var result = await appDbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == CategoryId);
        //    if (result != null)
        //    {
        //        appDbContext.Categories.Remove(result);
        //        await appDbContext.SaveChangesAsync();
        //        return result;
        //    }
        //    return null;
        //}

        public async Task<IEnumerable<Category>> GetCategories()
        {
            
            return await appDbContext.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryById(int CategoryId)
        {
            return await appDbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == CategoryId);
        }

        public async Task<Category> UpdateCategory(Category Category)
        {
            var result = await appDbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == Category.CategoryId);
            if (result != null)
            {
                result.Name = Category.Name;
                result.CategoryCode = Category.CategoryCode;
                result.IsActive = Category.IsActive;

                await appDbContext.SaveChangesAsync();
                return result;

            }

            return null;
        }
    }
}
