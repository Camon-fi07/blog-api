namespace weblog_API.Models.Tag;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreateTime { get; set; }
    public List<Post.Post> Posts { get; set; }
}