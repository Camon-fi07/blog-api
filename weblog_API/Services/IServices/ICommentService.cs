using weblog_API.Data.Dto;

namespace weblog_API.Services.IServices;

public interface ICommentService
{
    public Task<List<CommentDto>> GetPostComments(Guid postId);
    public Task<List<CommentDto>> GetAllNestedComments(Guid commentId);
    public Task AddComment(Guid postId, CreateCommentDto createCommentDto, string token);
    public Task EditComment(Guid commentId, string content, string token);
    public Task DeleteComment(Guid commentId, string token);
}