namespace weblog_API.Dto.Post;

public class PostDto
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int ReadingTime { get; set; }
    public string? Image { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; }
    public Guid? CommunityId { get; set; }
    public string? CommunityName { get; set; }
    public Guid? AddressId { get; set; }
    public int Likes { get; set; }
    public bool HasLike { get; set; }
    public int CommentsCount { get; set; }
    public List<TagDto> Tags { get; set; }
}