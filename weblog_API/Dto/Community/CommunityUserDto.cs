using weblog_API.Enums;

namespace weblog_API.Dto.Community;

public class CommunityUserDto
{
    public Guid UserId { get; set; }
    public Guid CommunityId { get; set; }
    public Role Role { get; set; }
}