using FruitSA.API.Auth.Model;
using FruitSA.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace FruitSA.Web.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;
    

        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
         
        }

        public async Task<UserManagerResponse> Login(LoginViewModel loginViewModel)
        {
           
            var apiUrl = "api/auth/login";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(loginViewModel), Encoding.UTF8,
                "application/json");
            var response = await httpClient.PostAsync(apiUrl, jsonContent);
            response.EnsureSuccessStatusCode();        

            var responseBody = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<UserManagerResponse>(responseBody);
           
            return responseObject;
            
        }

        public async Task<UserManagerResponse> Register(RegisterViewModel registerViewModel)
        {
            var apiUrl = "api/auth/register";
            var jsonContent = new StringContent(JsonConvert.SerializeObject(registerViewModel), Encoding.UTF8,
                "application/json");
            var response = await httpClient.PostAsync(apiUrl, jsonContent);
            response.EnsureSuccessStatusCode(); 

            var responseBody = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<UserManagerResponse>(responseBody);

            return responseObject;

        }
    }
}
