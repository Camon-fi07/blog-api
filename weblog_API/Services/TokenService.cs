using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using weblog_API.AppSettingsModels;
using weblog_API.Data;
using weblog_API.Middlewares;
using weblog_API.Models.User;
using weblog_API.Services.IServices;

namespace weblog_API.Services;

public class TokenService:ITokenService
{
    private readonly AppDbContext _db;
    private TokenProperties _tokenProperties;
    private JwtSecurityTokenHandler _tokenHandler;
    private readonly TokenValidationParameters _tokenValidationParameters;
    public TokenService(AppDbContext db, IConfiguration configuration)
    {
        _db = db;
        _tokenProperties = new TokenProperties();
        configuration.GetSection(nameof(TokenProperties)).Bind(_tokenProperties);
        _tokenHandler = new JwtSecurityTokenHandler();
        _tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenProperties.Secrets)),
            ValidIssuer = _tokenProperties.Issuer,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidAudience = _tokenProperties.Audience,
            ValidateAudience = true
        };
    }
    
    public string GetIdByToken(string token)
    {
        if (!_tokenHandler.CanReadToken(token)) throw new CustomException("Invalid token", 401);
        
        var jwtToken = _tokenHandler.ReadToken(token) as JwtSecurityToken;
        
        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value;
        if(userId == null) throw new CustomException("Invalid token", 401);
            
        return userId;
    }
    
    public string CreateToken(User user)
    {
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
        var token = _tokenHandler.CreateToken(tokenDescriptor);
        return _tokenHandler.WriteToken(token);
    }
    
    public async Task<bool> IsTokenBanned(string token)
    {
        return await _db.BannedTokens.AnyAsync(t => t.Token == token);
    }

    public bool ValidateToken(string? token)
    {
        try
        {
            _tokenHandler.ValidateToken(token,_tokenValidationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }
}