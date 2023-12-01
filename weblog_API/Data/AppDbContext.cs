using Microsoft.EntityFrameworkCore;
using weblog_API.Models;
using weblog_API.Models.Community;
using weblog_API.Models.User;

namespace weblog_API.Data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Community> Communities { get; set; }
    public DbSet<UserCommunity> UserCommunities { get; set; }
    public DbSet<TokenModel> BannedTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserCommunity>()
            .HasKey(uc => new { uc.UserId, uc.CommunityId });

        modelBuilder.Entity<UserCommunity>()
            .HasOne(uc => uc.User)
            .WithMany(u => u.Communities)
            .HasForeignKey(uc => uc.UserId);

        modelBuilder.Entity<UserCommunity>()
            .HasOne(uc => uc.Community)
            .WithMany(c => c.Subscribers)
            .HasForeignKey(uc => uc.CommunityId);
    }
}