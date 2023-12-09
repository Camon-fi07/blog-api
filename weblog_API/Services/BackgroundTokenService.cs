using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using weblog_API.AppSettingsModels;
using weblog_API.Data;

namespace weblog_API.Services;

public class BackgroundTokenService:BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private TokenProperties _tokenProperties;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly JwtSecurityTokenHandler _tokenHandler;
    public BackgroundTokenService(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _tokenHandler = new JwtSecurityTokenHandler();
        _tokenProperties = new TokenProperties();
        configuration.GetSection(nameof(TokenProperties)).Bind(_tokenProperties);
        _serviceProvider = serviceProvider;
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
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var bannedTokens = dbContext.BannedTokens;
                foreach (var token in bannedTokens)
                {
                    try
                    {
                        _tokenHandler.ValidateToken(token.Token, _tokenValidationParameters, out _);
                    }
                    catch
                    {
                        dbContext.BannedTokens.Remove(token);
                    }
                    
                }

                await dbContext.SaveChangesAsync();

            }
            
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}

