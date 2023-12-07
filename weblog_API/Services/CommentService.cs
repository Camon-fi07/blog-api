using Microsoft.EntityFrameworkCore;
using weblog_API.Data;
using weblog_API.Data.Dto;
using weblog_API.Mappers;
using weblog_API.Middlewares;
using weblog_API.Models.Comment;
using weblog_API.Services.IServices;

namespace weblog_API.Services;

public class CommentService:ICommentService
{
    private readonly AppDbContext _db;
    private readonly IUserService _userService;

    public CommentService(AppDbContext db, IUserService userService)
    {
        _db = db;
        _userService = userService;
    }

    public async Task<List<CommentDto>> GetPostComments(Guid postId)
    {
        var post = await _db.Posts.Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == postId);
        if (post == null) throw new CustomException("Can't find this post", 400);
        var comments = post.Comments.Select(c => CommentMapper.CommentToCommentDto(c));
        return comments.ToList();
    }

    public async Task<List<CommentDto>> GetAllNestedComments(Guid commentId)
    {
        var rootComment = await _db.Comments.Include(c => c.SubComments).FirstOrDefaultAsync(c => c.Id == commentId);
        if(rootComment == null) throw new CustomException("Can't find this comment", 400);
        var comments = rootComment.SubComments.Select(c => CommentMapper.CommentToCommentDto(c));
        return comments.ToList();
    }

    public async Task AddComment(Guid postId, CreateCommentDto createCommentDto, string token)
    {
        var post = await _db.Posts.Include(p => p.Comments).Include(p => p.Community).FirstOrDefaultAsync(p => p.Id == postId);
        if (post == null) throw new CustomException("Can't find this post", 400);
        var user = await _userService.GetUserByToken(token);
        
        if (post.Community != null && post.Community.IsClosed &&
            user.Communities.All(uc => uc.CommunityId != post.Community.Id))
        {
            throw new CustomException("User can't comment this post", 403);
        }
        
        var parentComment = createCommentDto.parentId == null ? null: await _db.Comments.FirstOrDefaultAsync(c => c.Id == createCommentDto.parentId);
        var comment = new Comment()
        {
            Id = Guid.NewGuid(),
            CreateTime = DateTime.UtcNow,
            Author = user,
            SubComments = new List<Comment>(),
            Content = createCommentDto.Content,
            Post = post,
            ParentComment = parentComment
        };
        _db.Comments.Add(comment);
        await _db.SaveChangesAsync();
    }

    public async Task EditComment(Guid commentId, string content, string token)
    {
        var comment = await _db.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
        if(comment == null) throw new CustomException("Can't find this comment", 400);
        var user = await _userService.GetUserByToken(token);
        if (comment.Author.Id != user.Id) throw new CustomException("User can't edit this comment", 403);
        comment.Content = content;
        comment.ModifiedDate = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteComment(Guid commentId, string token)
    {
        var comment = await _db.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
        if(comment == null) throw new CustomException("Can't find this comment", 400);
        var user = await _userService.GetUserByToken(token);
        if (comment.Author.Id != user.Id) throw new CustomException("User can't delete this comment", 403);
        if (comment.SubComments.Count > 0)
        {
            comment.Content = "Комментарий удалён";
            comment.DeleteDate = DateTime.UtcNow;
        }
        else _db.Comments.Remove(comment);
        await _db.SaveChangesAsync();
    }
}