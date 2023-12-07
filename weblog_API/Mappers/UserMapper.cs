using weblog_API.Data.Dto;
using weblog_API.Enums;
using weblog_API.Models.User;

namespace weblog_API.Mappers;

public static class UserMapper
{
    public static UserDto UserToUserDto(User user)
    {
        UserDto userDto = new UserDto()
        {
            Email = user.Email,
            Gender = Enum.GetName(typeof(Gender), user.Gender),
            FullName = user.FullName,
            Id = user.Id,
            createTime = user.CreateTime,
            Phone = user.PhoneNumber
        };
        return userDto;
    } 
    public static AuthorDto UserToAuthorDto(User user)
    {
        AuthorDto authorDto = new AuthorDto()
        {
            FullName = user.FullName,
            Gender = Enum.GetName(typeof(Gender), user.Gender),
            BirthDate = user.BirthDate,
            Posts = user.Posts.Count,
            Created = user.CreateTime,
            Likes = user.LikedPosts.Count
        };
        return authorDto;
    } 
}