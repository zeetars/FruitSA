using FruitSA.API.Auth.Model;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;

namespace FruitSA.Web.Components.Pages
{
    public class LoginBase: ComponentBase
    {
        [Parameter]
        public string Token { get; set; } = "";       
        [Inject]
        NavigationManager? NavigationManager { get; set; }
        [Inject]
        IUserService? userService { get; set; } 
        public LoginViewModel? User { get; set; } = new LoginViewModel();          
        public string errorMessage = "";
        public string successMessage = "";
        public async Task HandleLogin()
        {            
            errorMessage = "";
            successMessage = "";
            if (User==null)
            {
                errorMessage = "The Login Email and Passowrd is required.";
                return;
            }
            try
            {
                UserManagerResponse response = await userService.Login(User);
                if (response.IsSuccess)
                { 
                    var token = response.Message;
                    if (token != null)
                    {
                       NavigationManager.NavigateTo($"/{token}");
                    } 
                }
                else
                {
                    errorMessage = response.Message;                   
                    return;
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Login failed, please check your email/password and try again.";
                errorMessage = ex.Message;
                return;
            }
        }

        public void ForgotPassword()
        {
            // Redirect to forgot password page or handle recovery logic
            NavigationManager.NavigateTo("/forgot-password");
        }

        public void RegisterAccount()
        {
            // Redirect to register account page
            NavigationManager.NavigateTo("/register");
        }

    }
}
