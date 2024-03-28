using FruitSA.Model;
using FruitSA.Web.Components.Pages;
using Newtonsoft.Json;
using System.Net.Http;
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

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await httpClient.GetFromJsonAsync<Category[]>("api/categories");
        }

        public async Task<Category> GetCategoryById(int categoryId)
        {
            return await httpClient.GetFromJsonAsync<Category>($"api/categories/{categoryId}");
        }

        public async Task<Category> UpdateCategory(Category Category)
        {
            var apiUrl = "api/categories";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(Category), Encoding.UTF8,
                "application/json");
            var response = await httpClient.PutAsync(apiUrl, jsonContent);
            response.EnsureSuccessStatusCode();
            var updatedCategory = await response.Content.ReadFromJsonAsync<Category>();

            return updatedCategory;
        }


        public async Task<Category> CreateCategory(Category Category)
        {
            var apiUrl = "api/categories";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(Category), Encoding.UTF8,
                "application/json");
            var response = await httpClient.PostAsync(apiUrl, jsonContent);
            response.EnsureSuccessStatusCode();
            var createdCategory = await response.Content.ReadFromJsonAsync<Category>();

            return createdCategory;
        }

        //public async Task<Category> DeleteCategory(int categoryId)
        //{
        //    return await httpClient.DeleteFromJsonAsync<Category>($"api/categories/{categoryId}");
        //}

    }
}
