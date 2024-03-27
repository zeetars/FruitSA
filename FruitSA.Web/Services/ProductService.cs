using FruitSA.Model;
using System.Text;
using Newtonsoft.Json;

namespace FruitSA.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Product> GetProductById(int productId)
        {
            return await httpClient.GetFromJsonAsync<Product>($"api/products/{productId}");
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await httpClient.GetFromJsonAsync<Product[]>("api/products");
        }

        public async Task<Product> UpdateProduct(Product Product)
        {
            var apiUrl = "api/products";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(Product), Encoding.UTF8,
                "application/json");
            var response = await httpClient.PutAsync(apiUrl, jsonContent);
            response.EnsureSuccessStatusCode();
            var updatedProduct = await response.Content.ReadFromJsonAsync<Product>();

            return updatedProduct;
        }

        public async Task<Product> CreateProduct(Product Product)
        {
            var apiUrl = "api/products";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(Product), Encoding.UTF8,
                "application/json");
            var response = await httpClient.PostAsync(apiUrl, jsonContent);
            response.EnsureSuccessStatusCode();
            var createdProduct = await response.Content.ReadFromJsonAsync<Product>();

            return createdProduct;
        }



    }
}
