using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using weblog_API.AppSettingsModels;
using weblog_API.Data;
using weblog_API.Models.User;
using weblog_API.Services.IServices;

namespace weblog_API.Services;

public class TokenService:ITokenService
{
    private readonly AppDbContext _db;
    private TokenProperties _tokenProperties;
    public TokenService(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        _tokenProperties = new TokenProperties();
        configuration.GetSection(nameof(TokenProperties)).Bind(_tokenProperties);
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
    
    public string CreateToken(User user)
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
    
    public async Task<bool> IsTokenBanned(string token)
    {
        return await _db.BannedTokens.AnyAsync(t => t.Token == token);
    }
    
    public async Task<User> GetUserByToken(string token)
    {
        try
        {
            if (await IsTokenBanned(token)) throw new Exception("token is banned");
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
    
}