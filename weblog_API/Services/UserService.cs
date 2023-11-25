using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using weblog_API.AppSettingsModels;
using weblog_API.Data;
using weblog_API.Data.Dto;
using weblog_API.Models.User;
using weblog_API.Services.IServices;

namespace weblog_API.Services;

public class UserService:IUserService
{
    private readonly AppDbContext _db;
    private TokenProperties _tokenProperties;
    public UserService(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        _tokenProperties = new TokenProperties();
        configuration.GetSection(nameof(TokenProperties)).Bind(_tokenProperties);
    }

    public async Task<bool> isUniqueUser(string email)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user == null;
    }
    
    private string tokenCreation(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_tokenProperties.Secrets);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.Add(TimeSpan.Parse(_tokenProperties.TokenLifeTime)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _tokenProperties.Issuer,
            Audience = _tokenProperties.Audience
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string getIdByToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        if (!tokenHandler.CanReadToken(token)) throw new Exception("Invalid token");
        
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        
        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value;
            
        if(userId == null) throw new Exception("Invalid token");
            
        return userId;
    }
    
    private async Task<User> getUserByToken(string token)
    {
        try
        {
            var userId = getIdByToken(token);

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if(user == null) throw new Exception("Can't find user");
            return user;
        }
        catch(Exception)
        {
            throw;
        }
        
    }
    
    public async Task<TokenResponseDto> Registration(UserRegister registrationRequest)
    {
        User user = new()
        {
            Email = registrationRequest.Email,
            Gender = registrationRequest.Gender,
            Password = BCrypt.Net.BCrypt.HashPassword(registrationRequest.Password),
            BirthDate = registrationRequest.BirthDate,
            FullName = registrationRequest.FullName,
            PhoneNumber = registrationRequest.PhoneNumber,
            CreateTime = DateTime.UtcNow,
            Id = Guid.NewGuid()
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return new TokenResponseDto(){Token = tokenCreation(user)};
    }

    public async Task<TokenResponseDto> Login(LoginCredentials loginRequest)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
        if (user == null)
        {
            return new TokenResponseDto()
            {
                Token = ""
            };
        }

        if (BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
        {
            return new TokenResponseDto()
            {
                Token = tokenCreation(user)
            };
        }
        else return new TokenResponseDto()
        {
            Token = ""
        };
    }

    public async Task<UserDto> GetUser(string token)
    {
        try
        {
            var user = await getUserByToken(token);
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
        catch (Exception)
        {
            throw;
        }
       
    }

    public async Task Edit(UserEdit userEdit, string token)
    {
        try
        {
            var user = await getUserByToken(token);
            user.PhoneNumber = userEdit.PhoneNumber;
            user.BirthDate = userEdit.BirthDate;
            user.Gender = userEdit.Gender;
            user.Email = userEdit.Email;
            user.FullName = userEdit.FullName;
            await _db.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
       
    }
}