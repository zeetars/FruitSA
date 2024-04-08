using FruitSA.Model;

namespace FruitSA.Web.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategories(string authToken);      
        Task<Category> GetCategoryById(string authToken, int categoryId);
        Task<Category> UpdateCategory(string authToken, Category Category);
        Task<Category> CreateCategory(string authToken, Category Category);
    
    }
}
