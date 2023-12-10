namespace weblog_API.Dto.Comment;

public class CreateCommentDto
{
    public string Content { get; set; }
    public Guid? parentId { get; set; }
}
