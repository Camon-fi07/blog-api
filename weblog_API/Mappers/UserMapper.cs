using weblog_API.Data.Dto;
using weblog_API.Models.User;

namespace weblog_API.Mappers;

public static class UserMapper
{
    public static UserDto UserToUserDto(User user)
    {
        UserDto userDto = new UserDto()
        {
            Email = user.Email,
            Gender = user.Gender,
            FullName = user.FullName,
            Id = user.Id,
            createTime = user.CreateTime,
            Phone = user.PhoneNumber
        };
        return userDto;
    } 
}