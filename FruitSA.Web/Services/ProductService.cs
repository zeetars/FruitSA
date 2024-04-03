using FruitSA.Model;
using System.Text;
using Newtonsoft.Json;
using FruitSA.Web.Components.Pages;
using Microsoft.CodeAnalysis;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;

namespace FruitSA.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;
     

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
          
            //GetAccessToken();
        }
        public async Task<int> GetProductCount()
        {
            return await httpClient.GetFromJsonAsync<int>("api/products/count");
        }

        public async Task<bool> GetProductByCode(string productCode)
        {
            return await httpClient.GetFromJsonAsync<bool>($"api/products/{productCode}");
        }

        public async Task<Product> GetProductById(int productId)
        {
            return await httpClient.GetFromJsonAsync<Product>($"api/products/{productId}");
        }

        public async Task<IEnumerable<Product>> GetProducts(int pageNumber = 1, int pageSize = 10)
        {
          
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return await httpClient.GetFromJsonAsync<Product[]>($"api/products?pageNumber={pageNumber}&pageSize={pageSize}");
            
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

        public async Task<Product> DeleteProduct(int productId)
        {
            return await httpClient.DeleteFromJsonAsync<Product>($"api/products/{productId}");
        }

        public async Task<IEnumerable<Product>> CreateProducts(List<Product> Products)
        {
            var apiUrl = "api/uploads";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(Products), Encoding.UTF8,
                "application/json");
            var response = await httpClient.PostAsync(apiUrl, jsonContent);
            response.EnsureSuccessStatusCode();
            var allProducts = await this.GetProducts();

            return allProducts;

        }
      
    }
}
