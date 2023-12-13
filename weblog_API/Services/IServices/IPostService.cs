using weblog_API.Data.Dto;
using weblog_API.Dto.Post;
using weblog_API.Enums;

namespace weblog_API.Services.IServices;

public interface IPostService
{
    public Task<PostPagedListDto> GetPosts(List<Guid> tags, string? author, int? minReadingTime, int? maxReadingTime, PostSorting sorting,
         int page, int size, string? token, bool onlyMyCommunities);

    public Task<Guid> CreatePost(CreatePostDto createPostDto, string token, Guid? communityId);

    public Task<PostPagedListDto> GetCommunityPosts(Guid communityId, List<Guid> tags, string? author, int? minReadingTime,
        int? maxReadingTime, PostSorting sorting,
        int page, int size, string? token);

    public Task DeletePost(Guid id, string token);

    public Task<PostFullDto> GetConcretePost(Guid id, string? token);

    public Task AddLike(string token, Guid id);

    public Task DeleteLike(string token, Guid id);
}