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

    private Boolean IsCommentDeleted(Comment comment)
    {
        return comment.DeleteDate != null;
    }
    
    public async Task<List<CommentDto>> GetPostComments(Guid postId)
    {
        var post = await _db.Posts
            .Include(p => p.Comments).ThenInclude(c => c.SubComments)
            .Include(p => p.Comments).ThenInclude(c => c.Author)
            .FirstOrDefaultAsync(p => p.Id == postId);
        if (post == null) throw new CustomException("Can't find this post", 400);
        var comments = post.Comments.Where(c => c.ParentComment == null).Select(c => CommentMapper.CommentToCommentDto(c));
        return comments.ToList();
    }

    public async Task<List<CommentDto>> GetAllNestedComments(Guid commentId)
    {
        if(!_db.Comments.Any(c => c.Id == commentId)) throw new CustomException("Can't find this comment", 400);
        
        var comments = _db.Comments.Include(c => c.Author)
            .Include(c => c.Post)
            .Include(c => c.SubComments)
            .Include(c => c.ParentComment)
            .Where(c => c.ParentComment.Id == commentId)
            .Select(c => CommentMapper.CommentToCommentDto(c));
        
        return await comments.ToListAsync();
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
        
        if(parentComment == null && createCommentDto.parentId != null) throw new CustomException("Can't find parent comment", 400);
        if(parentComment != null && IsCommentDeleted(parentComment)) throw new CustomException("Parent comment was deleted", 400);
        
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
        if(IsCommentDeleted(comment)) throw new CustomException("This comment was deleted", 403);
        
        var user = await _userService.GetUserByToken(token);
        if (comment.Author.Id != user.Id) throw new CustomException("User can't edit this comment", 403);
        
        comment.Content = content;
        comment.ModifiedDate = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteComment(Guid commentId, string token)
    {
        var comment = await _db.Comments.Include(c => c.Author).Include(c => c.SubComments).FirstOrDefaultAsync(c => c.Id == commentId);
        
        if(comment == null) throw new CustomException("Can't find this comment", 400);
        if(IsCommentDeleted(comment)) throw new CustomException("This comment was deleted", 403);
        
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