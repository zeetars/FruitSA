using FruitSA.Model;

namespace FruitSA.Web.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategories();      
        Task<Category> GetCategoryById(int categoryId);
        Task<Category> UpdateCategory(Category Category);
        Task<Category> CreateCategory(Category Category);
        //Task<Category> DeleteCategory(int categoryId);
    }
}
