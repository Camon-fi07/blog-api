using System.ComponentModel.DataAnnotations;

namespace weblog_API.Data.Dto;

public class CreatePostDto
{
    [MinLength(5)]
    [MaxLength(1000)]
    public string Title { get; set; }
    [MinLength(5)]
    [MaxLength(5000)]
    public string Description { get; set; }
    public int ReadingTime { get; set; }
    public string? Image { get; set; }
    public Guid? AddressId { get; set; }
    public List<Guid> Tags { get; set; }
}