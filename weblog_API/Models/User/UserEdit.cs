using weblog_API.Enums;

namespace weblog_API.Models.User;

public class UserEdit
{
    public string Email { get; set; }
    public string FullName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string? PhoneNumber { get; set; }
}