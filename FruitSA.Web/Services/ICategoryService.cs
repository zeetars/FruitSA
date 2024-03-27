using FruitSA.Model;

namespace FruitSA.Web.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategories();
    }
}
