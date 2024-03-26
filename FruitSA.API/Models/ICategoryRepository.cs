using FruitSA.Model;

namespace FruitSA.API.Models
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> GetCategoryById(int CategoryId);
        Task<Category> AddCategory(Category Category);
        Task<Category> UpdateCategory(Category Category);
        Task<Category> DeleteCategory(int CategoryId);

    }
}
