using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using weblog_API.AppSettingsModels;
using weblog_API.Data;
using weblog_API.Data.Dto;
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
    private TokenProperties _tokenProperties;
    private ITokenService _tokenService;
    public UserService(AppDbContext db, IConfiguration configuration, ITokenService tokenService)
    {
        _db = db;
        _tokenProperties = new TokenProperties();
        configuration.GetSection(nameof(TokenProperties)).Bind(_tokenProperties);
        _tokenService = tokenService;
    }

    public async Task<bool> isUniqueUser(string email)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user == null;
    }
    
    public async Task<TokenModel> Registration(UserRegister registrationRequest)
    {
        var isUserUnique = await isUniqueUser(registrationRequest.Email);
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

    public async Task<UserDto> GetUser(string token)
    {
        try
        {
            var user = await _tokenService.GetUserByToken(token);
            return new UserDto()
            {
                Email = user.Email,
                Gender = user.Gender,
                FullName = user.FullName,
                Id = user.Id,
                createTime = user.CreateTime,
                Phone = user.PhoneNumber
            };
        }
        catch (CustomException)
        {
            throw;
        }
       
    }

    public async Task Edit(UserEdit userEdit, string token)
    {
        try
        {
            var user = await _tokenService.GetUserByToken(token);
            user.PhoneNumber = userEdit.PhoneNumber;
            user.BirthDate = userEdit.BirthDate;
            user.Gender = userEdit.Gender;
            user.Email = userEdit.Email;
            user.FullName = userEdit.FullName;
            await _db.SaveChangesAsync();
        }
        catch (CustomException)
        {
            throw;
        }
    }

    public async Task Logout(string token)
    {
        if (await _tokenService.IsTokenBanned(token)) throw new CustomException("Token has already banned", 401);
        _db.BannedTokens.Add(new TokenModel{Token = token});
        await _db.SaveChangesAsync();
    }
}