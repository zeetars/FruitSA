using FruitSA.Model;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FruitSA.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;     

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;          
        }

        public async Task<int> GetProductCount(string authToken)
        {
            TokenAuthorization(authToken);

            return await httpClient.GetFromJsonAsync<int>("api/products/count");
        }

        public async Task<bool> GetProductByCode(string authToken, string productCode)
        {
            TokenAuthorization(authToken);

            return await httpClient.GetFromJsonAsync<bool>($"api/products/{productCode}");
        }

        public async Task<Product> GetProductById(string authToken, int productId)
        {
            TokenAuthorization(authToken);

            return await httpClient.GetFromJsonAsync<Product>($"api/products/{productId}");
        }

        public async Task<IEnumerable<Product>> GetProducts(string authToken, int pageNumber = 1, int pageSize = 10)
        {
            TokenAuthorization(authToken);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            
            return await httpClient.GetFromJsonAsync<Product[]>($"api/products?pageNumber={pageNumber}&pageSize={pageSize}");
            
        }

        public async Task<Product> UpdateProduct(string authToken, Product Product)
        {
            TokenAuthorization(authToken);

            var apiUrl = "api/products";

            var jsonContent = new StringContent(JsonConvert.SerializeObject(Product), Encoding.UTF8,
                "application/json");

            var response = await httpClient.PutAsync(apiUrl, jsonContent);

            response.EnsureSuccessStatusCode();

            var updatedProduct = await response.Content.ReadFromJsonAsync<Product>();

            return updatedProduct;
           
        }

        public async Task<Product> CreateProduct(string authToken, Product Product)
        {
            TokenAuthorization(authToken);

            var apiUrl = "api/products";

            var jsonContent = new StringContent(JsonConvert.SerializeObject(Product), Encoding.UTF8,
                "application/json");

            var response = await httpClient.PostAsync(apiUrl, jsonContent);

            response.EnsureSuccessStatusCode();

            var createdProduct = await response.Content.ReadFromJsonAsync<Product>();

            return createdProduct;
        }

        public async Task<Product> DeleteProduct(string authToken, int productId)
        {
            TokenAuthorization(authToken);

            return await httpClient.DeleteFromJsonAsync<Product>($"api/products/{productId}");
        }

        public async Task<IEnumerable<Product>> CreateProducts(string authToken, List<Product> Products)
        {
            TokenAuthorization(authToken);
            var apiUrl = "api/uploads";

            var jsonContent = new StringContent(JsonConvert.SerializeObject(Products), Encoding.UTF8,
                "application/json");

            var response = await httpClient.PostAsync(apiUrl, jsonContent);

            response.EnsureSuccessStatusCode();

            var allProducts = await this.GetProducts(authToken);

            return allProducts;

        }

        private async Task TokenAuthorization(string authToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

    }
}
