using Microsoft.AspNetCore.Http.HttpResults;
using weblog_API.Enums;

namespace weblog_API.Data.Dto;

public class AuthorDto
{
    public string FullName { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Gender { get; set; }
    public int Posts { get; set; }
    public int Likes { get; set; }
    public DateTime Created { get; set; }
}