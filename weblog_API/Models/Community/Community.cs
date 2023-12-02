namespace weblog_API.Models.Community;

public class Community
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Boolean IsClosed { get; set; }
    public List<UserCommunity> Subscribers { get; set; }
    public List<Post.Post> Posts { get; set; }
}