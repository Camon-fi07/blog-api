using Microsoft.EntityFrameworkCore;
using weblog_API.Models;
using weblog_API.Models.User;

namespace weblog_API.Data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    
    }

    public DbSet<User> Users { get; set; }
    public DbSet<TokenModel> BannedTokens { get; set; }
}