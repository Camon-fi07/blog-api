using weblog_API.Enums;

namespace weblog_API.Data.Dto;

public class UserDto
{
    public Guid Id { get; set; }
    public DateTime createTime { get; set; }
    public string FullName { get; set; }
    public string Gender { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}