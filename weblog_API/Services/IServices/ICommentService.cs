using weblog_API.Dto.Comment;

namespace weblog_API.Services.IServices;

public interface ICommentService
{
    public Task<List<CommentDto>> GetPostComments(Guid postId, string? token);
    public Task<List<CommentDto>> GetAllNestedComments(Guid commentId, string? token);
    public Task AddComment(Guid postId, CreateCommentDto createCommentDto, string token);
    public Task EditComment(Guid commentId, UpdateCommentDto updateCommentDto, string token);
    public Task DeleteComment(Guid commentId, string token);
}