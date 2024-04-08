using FruitSA.Model;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FruitSA.Web.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient httpClient;

        public CategoryService(HttpClient httpClient) 
        { 
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<Category>> GetCategories(string authToken)
        {
            TokenAuthorization(authToken);
            return await httpClient.GetFromJsonAsync<Category[]>("api/categories");
        }

        public async Task<Category> GetCategoryById(string authToken, int categoryId)
        {
            TokenAuthorization(authToken);
            return await httpClient.GetFromJsonAsync<Category>($"api/categories/{categoryId}");
        }

        public async Task<Category> UpdateCategory(string authToken, Category Category)
        {
            TokenAuthorization(authToken);
            var apiUrl = "api/categories";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(Category), Encoding.UTF8,
                "application/json");
            var response = await httpClient.PutAsync(apiUrl, jsonContent);
            response.EnsureSuccessStatusCode();
            var updatedCategory = await response.Content.ReadFromJsonAsync<Category>();

            return updatedCategory;
        }


        public async Task<Category> CreateCategory(string authToken, Category Category)
        {
            TokenAuthorization(authToken);
            var apiUrl = "api/categories";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(Category), Encoding.UTF8,
                "application/json");
            var response = await httpClient.PostAsync(apiUrl, jsonContent);
            response.EnsureSuccessStatusCode();
            var createdCategory = await response.Content.ReadFromJsonAsync<Category>();

            return createdCategory;
        }

        private async Task TokenAuthorization(string authToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }

    }
}
