using weblog_API.Dto.User;
using weblog_API.Models.User;

namespace weblog_API.Services.IServices;

public interface IUserService
{
    public Task<User> GetUserByToken(string token);

    Task<TokenModel> Login(LoginCredentials loginRequest); 
    
    Task<TokenModel> Registration(UserRegisterDto registrationRequest);
    
    Task Edit(UserEditDto userEdit, string token);

    Task<UserDto> GetUser(string token);

    Task Logout(string token);
}