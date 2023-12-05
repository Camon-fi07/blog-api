using weblog_API.Models.Tags;

namespace weblog_API.Data.Dto;

public class CreatePostDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int ReadingTime { get; set; }
    public string? Image { get; set; }
    public Guid? AddressId { get; set; }
    public List<Guid> Tags { get; set; }
}