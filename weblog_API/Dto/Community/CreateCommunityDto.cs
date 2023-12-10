using System.ComponentModel.DataAnnotations;

namespace weblog_API.Dto.Community;

public class CreateCommunityDto
{
    [MinLength(1)]
    [MaxLength(100)]
    public string Name { get; set; }
    [MinLength(1)]
    [MaxLength(500)]
    public string? Description { get; set; }
    public Boolean IsClosed { get; set; }
}