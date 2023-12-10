using weblog_API.Dto.Community;
using weblog_API.Dto.User;
using weblog_API.Enums;
using weblog_API.Models.Community;
namespace weblog_API.Mappers;

public static class CommunityMapper
{
    public static CommunityDto CommunityToCommunityDto(Community community)
    {
        var communityDto = new CommunityDto()
        {
            Id = community.Id,
            Description = community.Description,
            IsClosed = community.IsClosed,
            Name = community.Name,
            CreateTime = community.CreateTime,
            SubscribersCount = community.Subscribers.Count
        };
        return communityDto;
    }
    public static CommunityFullDto CommunityToCommunityFullDto(Community community, List<UserDto> admins)
    {
        var communityDto = new CommunityFullDto()
        {
            Id = community.Id,
            Description = community.Description,
            IsClosed = community.IsClosed,
            Name = community.Name,
            CreateTime = community.CreateTime,
            SubscribersCount = community.Subscribers.Count,
            Administrators = admins
        };
        return communityDto;
    }
    public static CommunityUserDto UserCommunityToCommunityUserDto(UserCommunity community)
    {
        var communityUserDto = new CommunityUserDto()
        {
            CommunityId = community.CommunityId,
            UserId = community.UserId,
            Role = community.UserRole
        };
        return communityUserDto;
    }
}