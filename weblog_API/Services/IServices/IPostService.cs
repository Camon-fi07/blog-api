using weblog_API.Data.Dto;

namespace weblog_API.Services.IServices;

public interface IPostService
{
    public Task<List<PostDto>> GetPosts(string tag, string author, int minReadingTime, int maxReadingTime, string sorting,
        bool onlyMyCommunities, int page, int size);

    public Task CreatePost(CreatePostDto createPostDto, string token, Guid? communityId);

    public Task<PostFullDto> GetConcretePost(Guid id, string? token);

    public Task AddLike(string token, Guid id);

    public Task DeleteLike(string token, Guid id);
}