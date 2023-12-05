using weblog_API.Data.Dto;
using weblog_API.Models.Post;
using weblog_API.Models.User;

namespace weblog_API.Mappers;

public static class PostMapper
{
    public static PostDto PostToPostDto(Post post, User? user)
    {
        var postDto = new PostDto()
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
        };
        return postDto;
    }
    public static PostFullDto PostToPostFullDto(Post post, User? user, List<TagDto> tags, List<CommentDto> comments)
    {
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
}