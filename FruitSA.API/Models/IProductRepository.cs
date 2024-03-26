using FruitSA.Model;

namespace FruitSA.API.Models
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();        
        Task<Product> GetProductById(int ProductId);
        Task<Product> AddProduct(Product Product);
        Task<Product> UpdateProduct(Product Product);
        Task<Product> DeleteProduct(int ProductId);

    }
}
