namespace weblog_API.Dto.Comment;

public class CommentDto
{
    public Guid Id { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public DateTime? DeleteDate { get; set; }
    public DateTime CreateTime { get; set; }
    public string content { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; }
    public int SubComments { get; set; }
}