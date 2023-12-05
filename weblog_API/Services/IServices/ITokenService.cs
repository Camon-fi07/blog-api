using weblog_API.Models.User;

namespace weblog_API.Services.IServices;

public interface ITokenService
{
    public string CreateToken(User user);

    public Task<bool> IsTokenBanned(string token);

    public Task<User> GetUserByToken(string token);

    public bool ValidateToken(string? token);

}