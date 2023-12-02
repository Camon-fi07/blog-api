namespace weblog_API.Data.Dto;

public class PostFullDto:PostDto
{
    public List<CommentDto> Comments { get; set; }
}