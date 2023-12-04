using weblog_API.Data.Dto;
using weblog_API.Enums;

namespace weblog_API.Services.IServices;

public interface IPostService
{
    public Task<List<PostDto>> GetPosts(List<Guid> tags, string? author, int? minReadingTime, int? maxReadingTime, PostSorting sorting,
        bool onlyMyCommunities, int page, int size, string? token);

    public Task CreatePost(CreatePostDto createPostDto, string token, Guid? communityId);

    public Task<PostFullDto> GetConcretePost(Guid id, string? token);

    public Task AddLike(string token, Guid id);

    public Task DeleteLike(string token, Guid id);
}