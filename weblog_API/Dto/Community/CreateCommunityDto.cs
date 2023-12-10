namespace weblog_API.Dto.Community;

public class CreateCommunityDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public Boolean IsClosed { get; set; }
}