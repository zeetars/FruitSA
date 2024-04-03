using FruitSA.API.Auth.Model;

namespace FruitSA.Web.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> Login(LoginViewModel loginViewModel);
        Task<UserManagerResponse> Register(RegisterViewModel registerViewModel);
      
    }
}
