using weblog_API.Enums;

namespace weblog_API.Models.User;

public class User
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CreateTime { get; set; } 
}