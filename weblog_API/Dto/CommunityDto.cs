namespace weblog_API.Data.Dto;

public class CommunityDto
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Boolean IsClosed { get; set; }
    public int SubscribersCount { get; set; }
}