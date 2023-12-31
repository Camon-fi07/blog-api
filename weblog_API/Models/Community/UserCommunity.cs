using weblog_API.Enums;

namespace weblog_API.Models.Community;

public class UserCommunity
{
    public Guid UserId { get; set; }
    public Guid CommunityId { get; set; }
    public Community Community { get; set; }
    public User.User User { get; set; }
    public Role UserRole { get; set; }
}