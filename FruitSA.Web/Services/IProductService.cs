using FruitSA.Model;

namespace FruitSA.Web.Services
{
    public interface IProductService
    {
        Task<int> GetProductCount();
        Task<bool> GetProductByCode(string productCode);
        Task<IEnumerable<Product>> GetProducts(int pageNumber, int pageSize);
        Task<IEnumerable<Product>> CreateProducts(List<Product> Products);
        Task<Product> GetProductById(int productId);
        Task<Product> UpdateProduct(Product Product);
        Task<Product> CreateProduct(Product Product);
        Task<Product> DeleteProduct(int productId);
       
    }
}
