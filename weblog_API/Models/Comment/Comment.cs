using System.ComponentModel.DataAnnotations;

namespace weblog_API.Models.Comment;

public class Comment
{
    [Key]
    public Guid Id { get; set; }
    public Comment? ParentComment { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public DateTime? DeleteDate { get; set; }
    public DateTime CreateTime { get; set; }
    public Post.Post Post { get; set; }
    public User.User Author { get; set; }
    public List<Comment> SubComments { get; set; }
}