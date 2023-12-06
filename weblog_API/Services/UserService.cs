using Microsoft.EntityFrameworkCore;
using weblog_API.AppSettingsModels;
using weblog_API.Data;
using weblog_API.Data.Dto;
using weblog_API.Mappers;
using weblog_API.Middlewares;
using weblog_API.Models;
using weblog_API.Models.Community;
using weblog_API.Models.Post;
using weblog_API.Models.User;
using weblog_API.Services.IServices;

namespace weblog_API.Services;

public class UserService:IUserService
{
    private readonly AppDbContext _db;
    private readonly ITokenService _tokenService;
    public UserService(AppDbContext db, ITokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }

    private async Task<bool> IsUniqueUser(string email)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user == null;
    }
    
    public async Task<TokenModel> Registration(UserRegister registrationRequest)
    {
        var isUserUnique = await IsUniqueUser(registrationRequest.Email);
        if (!isUserUnique) throw new CustomException("There is already a user with this email", 400 );
        User user = new()
        {
            Email = registrationRequest.Email,
            Gender = registrationRequest.Gender,
            Password = BCrypt.Net.BCrypt.HashPassword(registrationRequest.Password),
            BirthDate = registrationRequest.BirthDate,
            FullName = registrationRequest.FullName,
            PhoneNumber = registrationRequest.PhoneNumber,
            CreateTime = DateTime.UtcNow,
            Id = Guid.NewGuid(),
            Communities = new List<UserCommunity>(),
            Posts = new List<Post>(),
            LikedPosts = new List<Post>()
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return new TokenModel(){Token =_tokenService.CreateToken(user)};
    }

    public async Task<TokenModel> Login(LoginCredentials loginRequest)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
        if (user == null) throw new CustomException("Invalid email or password", 401);

        if (BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
        {
            return new TokenModel()
            {
                Token = _tokenService.CreateToken(user)
            };
        }
        else return new TokenModel()
        {
            Token = ""
        };
    }

    public async Task<User> GetUserByToken(string token)
    {
        if (await _tokenService.IsTokenBanned(token)) throw new CustomException("Token is banned", 401);
        var userId = _tokenService.GetIdByToken(token);

        var user = await _db.Users.Include(u => u.Communities).ThenInclude(uc => uc.Community).ThenInclude(c => c.Posts).FirstOrDefaultAsync(u => u.Id.ToString() == userId);
        if(user == null) throw new CustomException("Can't find user", 401);
        return user;
    }
    
    public async Task<UserDto> GetUser(string token)
    {
        var user = await GetUserByToken(token);
        return UserMapper.UserToUserDto(user);
    }

    public async Task Edit(UserEdit userEdit, string token)
    {
        var user = await GetUserByToken(token);
        user.PhoneNumber = userEdit.PhoneNumber;
        user.BirthDate = userEdit.BirthDate;
        user.Gender = userEdit.Gender;
        user.Email = userEdit.Email;
        user.FullName = userEdit.FullName;
        await _db.SaveChangesAsync();
    }

    public async Task Logout(string token)
    {
        if (await _tokenService.IsTokenBanned(token)) throw new CustomException("Token has already banned", 401);
        _db.BannedTokens.Add(new TokenModel{Token = token});
        await _db.SaveChangesAsync();
    }
}