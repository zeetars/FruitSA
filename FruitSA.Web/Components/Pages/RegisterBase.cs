using FruitSA.API.Auth.Model;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Components;
using System.Xml;

namespace FruitSA.Web.Components.Pages
{
    public class RegisterBase : ComponentBase
    {
        [Inject]
        NavigationManager? NavigationManager { get; set; }
        [Inject]
        IUserService? userService { get; set; }
        public RegisterViewModel User { get; set; } = new RegisterViewModel();
        public UserManagerResponse userManagerResponse { get; set; }

        public string errorMessage = "";
        public string error = "";
        public async Task HandleRegistration()
        {
           
            errorMessage = "";
            error = "";
            
            if(User == null)
            {
                errorMessage = "The User Email and Passowrd is required.";
                return;
            }

            if(User.Password != User.ConfirmPassword)
            {
                errorMessage = "Failed Password Confirmation.";
                return;
            }

            try
            {
                UserManagerResponse response = await userService.Register(User);
                if (response.IsSuccess)
                {
                    userManagerResponse = response;
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    errorMessage = response.Errors.FirstOrDefault();
                    return;
                }
            }
            catch (Exception ex)
            {
                error = "Exception";
                return;
            }



        }

        public void LoginToAccount()
        {
            // Redirect to register account page
            NavigationManager.NavigateTo("/login");
        }
    }
}
