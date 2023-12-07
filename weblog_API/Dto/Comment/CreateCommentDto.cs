namespace weblog_API.Data.Dto;

public class CreateCommentDto
{
    public string Content { get; set; }
    public Guid? parentId { get; set; }
}
