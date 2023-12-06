using weblog_API.Data.Dto;
using weblog_API.Models;
using weblog_API.Models.User;

namespace weblog_API.Services.IServices;

public interface IUserService
{
    public Task<User> GetUserByToken(string token);

    Task<TokenModel> Login(LoginCredentials loginRequest); 
    
    Task<TokenModel> Registration(UserRegister registrationRequest);
    
    Task Edit(UserEdit userEdit, string token);

    Task<UserDto> GetUser(string token);

    Task Logout(string token);
}