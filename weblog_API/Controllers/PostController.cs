using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using weblog_API.Data.Dto;
using weblog_API.Services.IServices;

namespace weblog_API.Controllers;

[Route("api/[controller]")]
public class PostController : Controller
{
    private readonly IPostService _postService;
    public PostController(IPostService postService)
    {
        _postService = postService;
    }
    
    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostDto createPostDto)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        await _postService.CreatePost(createPostDto, token, null);
        return Ok();
    }    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PostFullDto>> GetPost(Guid id)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        var post = await _postService.GetConcretePost(id, token);
        return Ok(post);
    }    
}