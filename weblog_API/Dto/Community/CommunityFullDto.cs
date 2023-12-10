using weblog_API.Data.Dto;
using weblog_API.Dto.User;

namespace weblog_API.Dto.Community;

public class CommunityFullDto:CommunityDto
{
    public List<UserDto> Administrators { get; set; }
    
}