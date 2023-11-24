using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices.JavaScript;
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
    private string lifeTime;
    public UserRepository(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        secretKey = configuration.GetValue<string>("ApiSettings:Secrets");
        issuer= configuration.GetValue<string>("ApiSettings:Issuer");
        audience = configuration.GetValue<string>("ApiSettings:Audience");
        lifeTime = configuration.GetValue<string>("ApiSettings:TokenLifeTime");
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
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.Add(TimeSpan.Parse(lifeTime)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = issuer,
            Audience = audience
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
    
    private User getUserByToken(string token)
    {
        try
        {
            var userId = getIdByToken(token);

            var user = _db.Users.FirstOrDefault(u => u.Id.ToString() == userId);
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
        try
        {
            var user = getUserByToken(token);
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

    public void Edit(UserEdit userEdit, string token)
    {
        try
        {
            var user = getUserByToken(token);
            user.PhoneNumber = userEdit.PhoneNumber;
            user.BirthDate = userEdit.BirthDate;
            user.Gender = userEdit.Gender;
            user.Email = userEdit.Email;
            user.FullName = userEdit.FullName;
            _db.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
       
    }
}