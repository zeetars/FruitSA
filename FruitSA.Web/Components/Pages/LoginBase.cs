using FruitSA.API.Auth.Model;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;

namespace FruitSA.Web.Components.Pages
{
    public class LoginBase: ComponentBase
    {
        [Parameter]
        public string? Token { get; set; } = "";
        public static string authToken { get; set; } = "";
        [Inject]
        NavigationManager? NavigationManager { get; set; }
        [Inject]
        IUserService? userService { get; set; }
        public LoginViewModel? User { get; set; } = new LoginViewModel();
        public static string Email { get; set; } = "";  
        public string errorMessage = "";
        public string successMessage = "";

        protected override async Task OnInitializedAsync()
        {            
            try
            {
                if (Token != "")
                {
                    var handler = new JwtSecurityTokenHandler();
                    var decodedValue = handler.ReadJwtToken(Token);
                    var emailClaim = decodedValue.Claims.FirstOrDefault(claim => claim.Type == "Email");
                    Email = emailClaim.Value;
                    authToken = Token;
                }
            }
            catch (Exception)
            {
                NavigationManager.NavigateTo("/login");
            }
            
        }

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
                   
                    var message = response.Message;
                    if (message != null)
                    {
                        StateHasChanged();
                        NavigationManager.NavigateTo($"/{message}");
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
