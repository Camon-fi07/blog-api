using System.ComponentModel.DataAnnotations;
using weblog_API.Enums;

namespace weblog_API.Models.User;

public class UserRegister
{
    [Required]
    public string FullName { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Email { get; set; }
    public DateOnly BirthDate { get; set; }
    [Required]
    public Gender Gender { get; set; }
    public string PhoneNumber { get; set; }
}