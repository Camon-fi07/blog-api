namespace weblog_API.Dto.Post;

public class PostPagedListDto
{
    public List<PostDto> Posts { get; set; }
    public PageInfoDto Pagination { get; set; }
}