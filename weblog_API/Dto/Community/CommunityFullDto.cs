namespace weblog_API.Data.Dto;

public class CommunityFullDto:CommunityDto
{
    public List<UserDto> Administrators { get; set; }
    
}