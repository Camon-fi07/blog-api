using weblog_API.Data.Dto;
using weblog_API.Models.User;

namespace weblog_API.Repository.IRepository;

public interface IUserRepository
{
    bool isUniqueUser(string email);

    TokenResponseDto Login(LoginCredentials loginRequest); 
    
    Task<TokenResponseDto> Registration(UserRegister registrationRequest);

    UserDto? GetUser(string token);

}