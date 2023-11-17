using weblog_API.Data;
using weblog_API.Data.Dto;
using weblog_API.Models.User;
using weblog_API.Repository.IRepository;

namespace weblog_API.Repository;

public class UserRepository:IUserRepository
{
    private readonly AppDbContext _db;
    public string secretKey;

    public UserRepository(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        secretKey = configuration.GetValue<string>("ApiSettings:Secrets");
    }

    public bool isUniqueUser(string email)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == email);
        return user == null;
    }

    public async Task<TokenResponseDto> Registration(UserRegister registrationRequest)
    {
        User user = new()
        {
            Email = registrationRequest.Email,
            Gender = registrationRequest.Gender,
            Password = registrationRequest.Password,
            BirthDate = registrationRequest.BirthDate,
            FullName = registrationRequest.FullName,
            PhoneNumber = registrationRequest.PhoneNumber,
            CreateTime = DateTime.Now,
            Id = Guid.NewGuid()
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return new TokenResponseDto(){Token = ""};
    }

    public TokenResponseDto Login(LoginCredentials loginRequest)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == loginRequest.Email && u.Password == loginRequest.Password);
        if (user == null)
        {
            return new TokenResponseDto()
            {
                Token = ""
            };
        }
        return new TokenResponseDto()
        {
            Token = ""
        };
    }
}