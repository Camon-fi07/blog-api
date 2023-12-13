using Microsoft.EntityFrameworkCore;
using weblog_API.Data;
using weblog_API.Data.Dto;
using weblog_API.Dto.Post;
using weblog_API.Enums;
using weblog_API.Mappers;
using weblog_API.Middlewares;
using weblog_API.Models.Comment;
using weblog_API.Models.Post;
using weblog_API.Models.User;
using weblog_API.Services.IServices;

namespace weblog_API.Services;

public class PostService:IPostService
{
    private readonly AppDbContext _db;
    private readonly ITokenService _tokenService;
    private readonly ICommunityService _communityService;
    private readonly IUserService _userService;
    private readonly IAddressService _addressService;
    private readonly ITagsService _tagsService;
    public PostService(AppDbContext db, ITokenService tokenService, ICommunityService communityService, IUserService userService, IAddressService addressService, ITagsService tagsService)
    {
        _db = db;
        _tokenService = tokenService;
        _communityService = communityService;
        _userService = userService;
        _addressService = addressService;
        _tagsService = tagsService;
    }

    private  IQueryable<Post> GetAllPosts()
    {
        var posts = _db.Posts
            .Include(post => post.Comments).ThenInclude(comment => comment.Author)
            .Include(post => post.Comments).ThenInclude(comment => comment.SubComments)
            .Include(post => post.Tags)
            .Include(post => post.Community)
            .Include(post => post.UsersLiked)
            .Include(post => post.Author);
        return posts;
    }

    private IQueryable<Post> SortPosts(PostSorting sorting, IQueryable<Post> posts)
    {
        switch (sorting)
        {
            case PostSorting.CreateAsc:
                return posts.OrderBy(p => p.CreateTime);
            case PostSorting.CreateDesc:
                return posts.OrderByDescending(p => p.CreateTime);
            case PostSorting.LikeAsc:
                return posts.OrderBy(p => p.UsersLiked.Count);
            default:
                return posts.OrderByDescending(p => p.UsersLiked.Count);
        }
    }    
    
    public async Task<Guid> CreatePost(CreatePostDto createPostDto, string token, Guid? communityId)
    {
        if (createPostDto.Tags.Count == 0) throw new CustomException("Specify at least one tag for a new post",400);
        var posts = _db.Posts;
        var user = await _userService.GetUserByToken(token);
        _tagsService.CheckTags(createPostDto.Tags);
        var tags = _db.Tags.Where(t => createPostDto.Tags.Any(tp => tp == t.Id)).ToList();
        var community = communityId == null ? null : await _communityService.GetCommunityById((Guid)communityId);
        if (community != null && community.Subscribers.All(uc => uc.UserId != user.Id || uc.UserRole!=Role.Admin))
            throw new CustomException("User is not an admin of this community", 403);

        if (createPostDto.AddressId != null && !await _addressService.IsAddressAvailable((Guid)createPostDto.AddressId))
            throw new CustomException("Invalid address", 400);
        
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
        posts.Add(post);
        await _db.SaveChangesAsync();
        return post.Id;
    }

    public async Task DeletePost(Guid id, string token)
    {
        var posts = GetAllPosts().ToList();
        var post = posts.FirstOrDefault(p => p.Id == id);
        if (post == null) throw new CustomException("There is not a post with this Id", 400);
        var user = await _userService.GetUserByToken(token);
        var userCommunity = user.Communities.FirstOrDefault(c => c.CommunityId == post.Community?.Id);
        if (post.Author.Id != user.Id && userCommunity?.UserRole != Role.Admin) throw new CustomException("User don't have access to this post", 403);
        posts.Remove(post);
        _db.Entry(post).State = EntityState.Deleted;
        await _db.SaveChangesAsync();
    }

    private IQueryable<Post> FilterClosedCommunities(List<Guid> userCommunities, IQueryable<Post> posts)
    {
        return posts.Where(p => p.Community == null || !p.Community.IsClosed || userCommunities.Contains(p.Community.Id));
    }
    
    private  IQueryable<Post> FilterPosts(IQueryable<Post> posts, List<Guid> tags, string? author, int? minReadingTime, int? maxReadingTime,
        PostSorting sorting, int page, int size)
    {
        var filterPosts = posts;
        if(tags.Count > 0) filterPosts = filterPosts.Where(p => p.Tags.Any(t => tags.Any(id => t.Id == id)));
        if(author != null) filterPosts = filterPosts.Where(p => p.Author.FullName.Contains(author));
        if(minReadingTime != null) filterPosts = filterPosts.Where(p => minReadingTime <= p.ReadingTime);
        if(maxReadingTime != null) filterPosts = filterPosts.Where(p => p.ReadingTime <= maxReadingTime);
        filterPosts = SortPosts(sorting, filterPosts);
        return filterPosts;
    }
    
    public async Task<PostPagedListDto> GetCommunityPosts(Guid communityId, List<Guid> tags, string? author, int? minReadingTime, int? maxReadingTime, PostSorting sorting,
        int page, int size, string? token)
    {
        if (minReadingTime != null && maxReadingTime != null && maxReadingTime < minReadingTime)
            throw new CustomException("Max reading time couldn't be less than min reading time", 400);
        _tagsService.CheckTags(tags);
        
        var posts = GetAllPosts();
        User? user = null;
        if(_tokenService.ValidateToken(token)) user = await _userService.GetUserByToken(token);
        var community = await _communityService.GetCommunityById(communityId);

        if (user != null && community.IsClosed && community.Subscribers.All(uc => uc.User.Id != user.Id))
            throw new CustomException("User is not a subscriber of this group", 403);
        
        var communityPosts = posts.Where(p => p.Community != null && p.Community.Id == communityId);
        
        communityPosts = FilterPosts(communityPosts, tags,  author,  minReadingTime,  maxReadingTime,  sorting,
             page, size);
        var pagedPosts = communityPosts.Skip((page - 1) * size).Take(size).ToList();
        int count = communityPosts.Count() / (page * size) == 0 ? 1 : communityPosts.Count() / (page * size);
        return PostMapper.PostsToPostPagedListDto(pagedPosts, user, page,count, size);
    }
    
    public async Task<PostPagedListDto> GetPosts(List<Guid> tags, string? author, int? minReadingTime, int? maxReadingTime, PostSorting sorting,
        int page, int size, string? token, bool onlyMyCommunities)
    {
        if (minReadingTime != null && maxReadingTime != null && maxReadingTime < minReadingTime)
            throw new CustomException("Max reading time couldn't be less than min reading time", 400);
        _tagsService.CheckTags(tags);
        
        User? user = null;
        if(_tokenService.ValidateToken(token)) user = await _userService.GetUserByToken(token);
        var posts = GetAllPosts();
        
        if (user != null)
        {
            var userCommunities = user.Communities.Select(u => u.CommunityId).ToList();
            posts = FilterClosedCommunities(userCommunities, posts);
            if(onlyMyCommunities) posts = posts.Where(p => p.Community != null && userCommunities.Contains(p.Community.Id));
        }
        posts = FilterPosts(posts,tags,  author,  minReadingTime,  maxReadingTime,  sorting, page, size);
        int count = posts.Count() / (page * size) == 0 ? 1 : posts.Count() / (page * size);
        var pagedPosts = posts.Skip((page - 1) * size).Take(size).ToList();
        return PostMapper.PostsToPostPagedListDto(pagedPosts, user, page,count, size);
    }

    public async Task<PostFullDto> GetConcretePost(Guid id, string? token)
    {
        var post = await GetAllPosts().FirstOrDefaultAsync(p => p.Id == id);
        
        if (post == null) throw new CustomException("There is not a post with this Id", 400);
        
        var comments = post.Comments.Select(c => CommentMapper.CommentToCommentDto(c)).ToList();
        
        var tags = post.Tags.Select(t => TagMapper.TagToTagDto(t)).ToList();
        
        User? user = null;
        if(_tokenService.ValidateToken(token)) user = await _userService.GetUserByToken(token);
        if (post.Community != null && post.Community.IsClosed &&
            (user == null || user.Communities.All(uc => uc.CommunityId != post.Community.Id)))
            throw new CustomException("You don't have rights", 403);
        
        return PostMapper.PostToPostFullDto(post, user, tags, comments);
    }

    public async Task AddLike(string token, Guid id)
    {
        var post = await _db.Posts.Include(p => p.UsersLiked).Include(p => p.Community).FirstOrDefaultAsync(p => p.Id == id);
        if (post == null) throw new CustomException("There is not a post with this Id", 400);
        var user = await _userService.GetUserByToken(token);
        if (post.UsersLiked.Any(u => u.Id == user.Id)) throw new CustomException("User has already liked this post", 400);
        if(post.Community != null && post.Community.IsClosed && user.Communities.All(c => c.UserId!=user.Id)) 
            throw new CustomException("User can't add like to this post", 403);
        post.UsersLiked.Add(user);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteLike(string token, Guid id)
    {
        var post = await _db.Posts.Include(p => p.UsersLiked).FirstOrDefaultAsync(p => p.Id == id);
        if (post == null) throw new CustomException("There is not a post with this Id", 400);
        var user = await _userService.GetUserByToken(token);
        if (post.UsersLiked.All(u => u.Id != user.Id)) throw new CustomException("User don't like this post", 400);
        post.UsersLiked.Remove(user);
        await _db.SaveChangesAsync();
    }
}