using System.ComponentModel.DataAnnotations;
using weblog_API.Models.Tags;

namespace weblog_API.Models.Post;

public class Post
{
    [Key]
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int ReadingTime { get; set; }
    public string? Image { get; set; }
    public List<User.User> UsersLiked { get; set; }
    public List<Tag> Tags { get; set; }
    public User.User Author { get; set; }
    public Community.Community? Community { get; set; }
    public List<Comment.Comment> Comments { get; set; }
    public Guid? AddressId { get; set; }
}