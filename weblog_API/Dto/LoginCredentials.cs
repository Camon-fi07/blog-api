using System.ComponentModel.DataAnnotations;

namespace weblog_API.Data.Dto;

public class LoginCredentials
{
    [Required] 
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}