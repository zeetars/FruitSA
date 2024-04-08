using FruitSA.Model;

namespace FruitSA.Web.Services
{
    public interface IProductService
    {
        Task<int> GetProductCount(string authToken);
        Task<bool> GetProductByCode(string authToken,  string productCode);
        Task<IEnumerable<Product>> GetProducts(string authToken, int pageNumber, int pageSize);
        Task<IEnumerable<Product>> CreateProducts(string authToken, List<Product> Products);
        Task<Product> GetProductById(string authToken,  int productId);
        Task<Product> UpdateProduct(string authToken, Product Product);
        Task<Product> CreateProduct(string authToken, Product Product);
        Task<Product> DeleteProduct(string authToken, int productId);
       
    }
}
