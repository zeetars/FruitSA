using FruitSA.Model;

namespace FruitSA.Web.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProductById(int productId);
        Task<Product> UpdateProduct(Product Product);
        Task<Product> CreateProduct(Product Product);
    }
}
