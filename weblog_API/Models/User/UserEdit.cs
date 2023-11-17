using System.ComponentModel.DataAnnotations;
using weblog_API.Enums;

namespace weblog_API.Models.User;

public class UserEdit
{
    [Required]
    public string Email { get; set; }
    [Required] 
    public string FullName { get; set; }
    public DateOnly BirthDate { get; set; } //почему в swagger используется DateTime
    [Required]
    public Gender Gender { get; set; }
    public string PhoneNumber { get; set; }
    
    
}