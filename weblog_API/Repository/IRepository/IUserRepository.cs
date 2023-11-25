using weblog_API.Data.Dto;
using weblog_API.Models.User;

namespace weblog_API.Repository.IRepository;

public interface IUserRepository
{
    Task<bool> isUniqueUser(string email);

    Task<TokenResponseDto> Login(LoginCredentials loginRequest); 
    
    Task<TokenResponseDto> Registration(UserRegister registrationRequest);
    
    Task Edit(UserEdit userEdit, string token);

    Task<UserDto> GetUser(string token);

}