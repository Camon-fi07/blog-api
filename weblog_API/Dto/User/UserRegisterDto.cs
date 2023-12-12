using System.ComponentModel.DataAnnotations;
using weblog_API.Enums;

namespace weblog_API.Dto.User;

public class UserRegisterDto
{
    [Required]
    public string FullName { get; set; }
    [Required]
    public string Password { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public DateOnly? BirthDate { get; set; }
    public Gender Gender { get; set; }
    [Required]
    [RegularExpression(@"^\+7 \([0-9]{3}\) [0-9]{3} [0-9]{2} [0-9]{2}$")]
    public string? PhoneNumber { get; set; }
}