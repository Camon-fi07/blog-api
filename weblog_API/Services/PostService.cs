using Microsoft.EntityFrameworkCore;
using weblog_API.Data;
using weblog_API.Data.Dto;
using weblog_API.Middlewares;
using weblog_API.Models.Comment;
using weblog_API.Models.Post;
using weblog_API.Models.Tags;
using weblog_API.Models.User;
using weblog_API.Services.IServices;

namespace weblog_API.Services;

public class PostService:IPostService
{
    private readonly AppDbContext _db;
    private readonly ITokenService _tokenService;
    private readonly ICommunityService _communityService;
    public PostService(AppDbContext db, ITokenService tokenService, ICommunityService communityService)
    {
        _db = db;
        _tokenService = tokenService;
        _communityService = communityService;
    }

    public async Task CreatePost(CreatePostDto createPostDto, string token, Guid? communityId)
    {
        var posts = _db.Posts.ToList();
        var user = await _tokenService.GetUserByToken(token);
        var tags = _db.Tags.Where(t => createPostDto.Tags.Any(tp => tp == t.Id)).ToList();
        var community = communityId == null ? null : await _communityService.GetCommunityById((Guid)communityId);
        var post = new Post()
        {
            Id = Guid.NewGuid(),
            Description = createPostDto.Description,
            CreateTime = DateTime.UtcNow,
            Author = user,
            Comments = new List<Comment>(),
            Tags = tags,
            ReadingTime = createPostDto.ReadingTime,
            UsersLiked = new List<User>(),
            Title = createPostDto.Title,
            AddressId = createPostDto.AddressId,
            Image = createPostDto.Image,
            Community = community
        };
        _db.Entry(post).State = EntityState.Added;
        posts.Add(post);
        user.Posts.Add(post);
        community?.Posts.Add(post);
        await _db.SaveChangesAsync();
    }
    
    public Task<List<PostDto>> GetPosts(string tag, string author, int minReadingTime, int maxReadingTime, string sorting, bool onlyMyCommunities,
        int page, int size)
    {
        throw new NotImplementedException();
    }

    public async Task<PostFullDto> GetConcretePost(Guid id, string? token)
    {
        var post = await _db.Posts
            .Include(post => post.Comments).ThenInclude(comment => comment.Author)
            .Include(post => post.Comments).ThenInclude(comment => comment.SubComments)
            .Include(post => post.Tags)
            .Include(post => post.Community)
            .Include(post => post.UsersLiked)
            .Include(post => post.Author)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (post == null) throw new CustomException("There is not a post with this Id", 400);
        
        var comments = post.Comments.Select(c => new CommentDto()
        {
            Id = c.Id,
            CreateTime = c.CreateTime,
            AuthorId = c.Author.Id,
            AuthorName = c.Author.FullName,
            DeleteDate = c.DeleteDate,
            ModifiedDate = c.ModifiedDate,
            SubComments = c.SubComments.Count
        }).ToList();
        
        var tags = post.Tags.Select(t => new TagDto()
        {
            Id = t.Id,
            CreateTime = t.CreateTime,
            Name = t.Name
        }).ToList();
        
        var user = token == null ? null : await _tokenService.GetUserByToken(token);

        var postDto = new PostFullDto()
        {
            Id = post.Id,
            Description = post.Description,
            CreateTime = post.CreateTime,
            Comments = comments,
            AddressId = post.AddressId,
            Image = post.Image,
            AuthorId = post.Author.Id,
            AuthorName = post.Author.FullName,
            ReadingTime = post.ReadingTime,
            Tags = tags,
            CommunityId = post.Community?.Id,
            CommunityName = post.Community?.Name,
            Title = post.Title,
            Likes = post.UsersLiked.Count,
            CommentsCount = comments.Count,
            HasLike = user != null && post.UsersLiked.Any(u => u.Id == user.Id),
        };
        return postDto;
    }

    public Task AddLike(string token, Guid id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteLike(string token, Guid id)
    {
        throw new NotImplementedException();
    }
}