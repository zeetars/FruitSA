using Azure;
using Blazored.LocalStorage;
using FruitSA.API.Auth.Model;
using FruitSA.Web.Models;
using FruitSA.Web.Providers;
using FruitSA.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.JSInterop;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FruitSA.Web.Components.Pages
{
    public class LoginBase: ComponentBase
    {
        [Inject]
        NavigationManager? NavigationManager { get; set; }
        [Inject]
        IUserService? userService { get; set; }
        public LoginViewModel? User { get; set; } = new LoginViewModel();
        [Inject]
        IJSRuntime jsRuntime { get; set; }
        public UserManagerResponse userManagerResponse { get; set; } = new UserManagerResponse();

        //Using the Provider Class for local storage.
        [Inject]
        LocalStorageAccessor LocalStorageAccessor {  get; set; }
        public string Key { get; set; } = "";
        public string Value { get; set; } = "";
        public string StoredValue { get; set; } = "";


        public string errorMessage = "";
        public string successMessage = "";
        protected override async Task OnInitializedAsync()
        {
            
            LocalStorageAccessor = new LocalStorageAccessor(jsRuntime);
            

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
                   
                    userManagerResponse = response;
                    var message = userManagerResponse.Message;
                    if (message != null)
                    {
                        Key = "authToken";
                        Value = message;                        
                       
                        await LocalStorageAccessor.SetValueAsync(Key, Value);
                        StoredValue = await LocalStorageAccessor.GetValueAsync<string>(Key);
                        NavigationManager.NavigateTo("/");
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
