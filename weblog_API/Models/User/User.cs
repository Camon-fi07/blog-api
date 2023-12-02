using weblog_API.Enums;
using weblog_API.Models.Community;

namespace weblog_API.Models.User;

public class User
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CreateTime { get; set; }
    public List<UserCommunity> Communities { get; set; }
    public List<Post.Post> Posts { get; set; }
    public List<Post.Post> LikedPosts { get; set; }
}