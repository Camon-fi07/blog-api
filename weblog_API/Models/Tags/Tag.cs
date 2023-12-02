namespace weblog_API.Models.Tags;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreateTime { get; set; }
}