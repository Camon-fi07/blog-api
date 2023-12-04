using Microsoft.EntityFrameworkCore;
using weblog_API.Data;
using weblog_API.Data.Dto;
using weblog_API.Enums;
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


    private List<Post> SortPosts(PostSorting sorting, List<Post> posts)
    {
        switch (sorting)
        {
            case PostSorting.CreateAsc:
                return posts.OrderBy(p => p.CreateTime).ToList();
            case PostSorting.CreateDesc:
                return posts.OrderByDescending(p => p.CreateTime).ToList();
            case PostSorting.LikeAsc:
                return posts.OrderBy(p => p.UsersLiked.Count).ToList();
            default:
                return posts.OrderByDescending(p => p.UsersLiked.Count).ToList();
        }
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
    
    public async Task<List<PostDto>> GetPosts(List<Guid> tags, string? author, int? minReadingTime, int? maxReadingTime, PostSorting sorting, bool onlyMyCommunities,
        int page, int size, string? token)
    {
        var user = token == null ? null : await _tokenService.GetUserByToken(token);
        var posts = _db.Posts
            .Include(post => post.Comments).ThenInclude(comment => comment.Author)
            .Include(post => post.Comments).ThenInclude(comment => comment.SubComments)
            .Include(post => post.Tags)
            .Include(post => post.Community)
            .Include(post => post.UsersLiked)
            .Include(post => post.Author);
        
        var filterPosts = posts.Skip((page-1)*size).Take(size);
        if(tags.Count > 0) filterPosts = filterPosts.Where(p => p.Tags.Any(t => tags.Any(id => t.Id == id)));
        if(author != null) filterPosts = filterPosts.Where(p => p.Author.FullName.Contains(author));
        if(minReadingTime != null) filterPosts = filterPosts.Where(p => minReadingTime <= p.ReadingTime);
        if(maxReadingTime != null) filterPosts = filterPosts.Where(p => p.ReadingTime <= maxReadingTime); 
        if(user != null && onlyMyCommunities) filterPosts = filterPosts.Where(p => p.Community== null || user.Communities.Any(c => c.CommunityId == p.Community.Id));
        var sortPosts = SortPosts(sorting, filterPosts.ToList()).Select(post => new PostDto()
        {
            Id = post.Id,
            Description = post.Description,
            CreateTime = post.CreateTime,
            AddressId = post.AddressId,
            Image = post.Image,
            AuthorId = post.Author.Id,
            AuthorName = post.Author.FullName,
            ReadingTime = post.ReadingTime,
            Tags = post.Tags.Select(t => new TagDto()
            {
                Id = t.Id,
                CreateTime = t.CreateTime,
                Name = t.Name
            }).ToList(),
            CommunityId = post.Community?.Id,
            CommunityName = post.Community?.Name,
            Title = post.Title,
            Likes = post.UsersLiked.Count,
            CommentsCount = post.Comments.Count,
            HasLike = user != null && post.UsersLiked.Any(u => u.Id == user.Id),
        }).ToList();
        return sortPosts;
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

    public async Task AddLike(string token, Guid id)
    {
        var post = await _db.Posts.Include(p => p.UsersLiked).FirstOrDefaultAsync(p => p.Id == id);
        if (post == null) throw new CustomException("There is not a post with this Id", 400);
        var user = await _tokenService.GetUserByToken(token);
        if (post.UsersLiked.Any(u => u.Id == user.Id)) throw new CustomException("User has already liked this post", 400);
        post.UsersLiked.Add(user);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteLike(string token, Guid id)
    {
        var post = await _db.Posts.Include(p => p.UsersLiked).FirstOrDefaultAsync(p => p.Id == id);
        if (post == null) throw new CustomException("There is not a post with this Id", 400);
        var user = await _tokenService.GetUserByToken(token);
        if (!post.UsersLiked.Any(u => u.Id == user.Id)) throw new CustomException("User don't like this post", 400);
        post.UsersLiked.Remove(user);
        await _db.SaveChangesAsync();
    }
}