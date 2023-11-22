using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using weblog_API.Data;
using weblog_API.Data.Dto;
using weblog_API.Models.User;
using weblog_API.Repository.IRepository;

namespace weblog_API.Repository;

public class UserRepository:IUserRepository
{
    private readonly AppDbContext _db;
    private string secretKey;
    private string issuer;
    private string audience;
    public UserRepository(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        secretKey = configuration.GetValue<string>("ApiSettings:Secrets");
        issuer= configuration.GetValue<string>("ApiSettings:Issuer");
        audience = configuration.GetValue<string>("ApiSettings:Audience");
    }

    public bool isUniqueUser(string email)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == email);
        return user == null;
    }

    private string tokenCreation(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = issuer,
            Audience = audience
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private User? getUserByToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        if (!tokenHandler.CanReadToken(token)) return null;
        
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        
        var userEmail = jwtToken.Claims.FirstOrDefault(c => c.Type == "email").Value;
        if (userEmail == null) return null;
        
        var user = _db.Users.FirstOrDefault(u => u.Email == userEmail);

        return user;
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

    public TokenResponseDto Login(LoginCredentials loginRequest)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == loginRequest.Email);
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

    public UserDto? GetUser(string token)
    {
        var user = getUserByToken(token);
        if (user == null) return null;
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

    public bool Edit(UserEdit userEdit, string token)
    {
        var user = getUserByToken(token);
        if (user == null) return false;
        user.PhoneNumber = userEdit.PhoneNumber;
        user.BirthDate = userEdit.BirthDate;
        user.Gender = userEdit.Gender;
        user.Email = userEdit.Email;
        user.FullName = userEdit.FullName;
        _db.SaveChanges();
        return true;
    }
}