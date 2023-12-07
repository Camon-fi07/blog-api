using Microsoft.AspNetCore.Mvc;
using weblog_API.Data.Dto;
using weblog_API.Services.IServices;

namespace weblog_API.Controllers;

[Route("api/Author")]
[ApiController]
public class AuthorController : Controller
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }
    
    [HttpGet]
    public ActionResult<List<AuthorDto>> GetAuhtorList()
    {
        var authors = _authorService.GetAuthorList();
        return Ok(authors);
    }
}