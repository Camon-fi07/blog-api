namespace weblog_API.Data.Dto;

public class CommunityUserDto
{
    public Guid UserId { get; set; }
    public Guid CommunityId { get; set; }
    public string Role { get; set; }
}