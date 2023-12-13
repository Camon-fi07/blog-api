using System.ComponentModel.DataAnnotations;

namespace weblog_API.Models.User;

public class TokenModel
{
    [Key]
    public string Token { get; set; }
}