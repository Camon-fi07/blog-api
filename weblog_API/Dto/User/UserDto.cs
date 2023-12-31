using weblog_API.Enums;

namespace weblog_API.Dto.User;

public class UserDto
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string FullName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
}