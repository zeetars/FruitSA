using FruitSA.Model;

namespace FruitSA.API.Models
{
    public interface IUploadRepository
    {
        Task AddProducts(List<Product> Products);
    }
}
