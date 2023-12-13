using System.ComponentModel.DataAnnotations;

namespace weblog_API.Dto.User;

public class LoginCredentials
{
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}