using Microsoft.EntityFrameworkCore;
using weblog_API.Models;
using weblog_API.Models.Comment;
using weblog_API.Models.Community;
using weblog_API.Models.Post;
using weblog_API.Models.Tags;
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
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    
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

        modelBuilder.Entity<User>()
            .HasMany(u => u.Posts)
            .WithOne(p => p.Author);

        modelBuilder.Entity<Post>()
            .HasMany(p => p.UsersLiked)
            .WithMany(u => u.LikedPosts)
            .UsingEntity("Likes");

        modelBuilder.Entity<Community>()
            .HasMany(c => c.Posts)
            .WithOne(p => p.Community);

        modelBuilder.Entity<Post>()
            .HasMany(p => p.Comments)
            .WithOne(c => c.Post);

        modelBuilder.Entity<Comment>()
            .HasMany(c => c.SubComments)
            .WithOne(c => c.ParentComment);
        
        
        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = Guid.NewGuid(), Name = "история", CreateTime = DateTime.UtcNow, Posts = new List<Post>()},
            new Tag { Id = Guid.NewGuid(), Name = "еда", CreateTime = DateTime.UtcNow, Posts = new List<Post>() },
            new Tag { Id = Guid.NewGuid(), Name = "18+", CreateTime = DateTime.UtcNow, Posts = new List<Post>() },
            new Tag { Id = Guid.NewGuid(), Name = "приколы", CreateTime = DateTime.UtcNow, Posts = new List<Post>() },
            new Tag { Id = Guid.NewGuid(), Name = "it", CreateTime = DateTime.UtcNow, Posts = new List<Post>() },
            new Tag { Id = Guid.NewGuid(), Name = "интернет", CreateTime = DateTime.UtcNow, Posts = new List<Post>() },
            new Tag { Id = Guid.NewGuid(), Name = "теория_заговора", CreateTime = DateTime.UtcNow, Posts = new List<Post>() },
            new Tag { Id = Guid.NewGuid(), Name = "соцсети", CreateTime = DateTime.UtcNow, Posts = new List<Post>() },
            new Tag { Id = Guid.NewGuid(), Name = "косплей", CreateTime = DateTime.UtcNow, Posts = new List<Post>() },
            new Tag { Id = Guid.NewGuid(), Name = "преступление", CreateTime = DateTime.UtcNow, Posts = new List<Post>() }
        );
    }
}