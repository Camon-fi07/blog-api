using System.ComponentModel.DataAnnotations;

namespace weblog_API.Dto.Comment;

public class CreateCommentDto
{
    [MinLength(1)]
    [MaxLength(1000)]
    public string Content { get; set; }
    public Guid? parentId { get; set; }
}
