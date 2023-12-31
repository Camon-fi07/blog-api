using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using weblog_API.Data.Dto;
using weblog_API.Dto.Post;
using weblog_API.Enums;
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
    public async Task<ActionResult<Guid>> CreatePost([FromBody] CreatePostDto createPostDto)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        var postId = await _postService.CreatePost(createPostDto, token, null);
        return Ok(postId);
    }    
    
    [HttpDelete("delete")]
    [Authorize]
    public async Task<IActionResult> DeletePost(Guid id)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        await _postService.DeletePost(id, token);
        return Ok();
    }    
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PostFullDto>> GetPost(Guid id)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        var post = await _postService.GetConcretePost(id, token);
        return Ok(post);
    }    
    
    [HttpGet("")]
    public async Task<ActionResult<PostPagedListDto>> GetPosts(List<Guid> tags, string? author, int? minReadingTime, int? maxReadingTime, PostSorting sorting,
        int page=1, int size=5, bool onlyMyCommunities=false)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        var posts = await _postService.GetPosts(tags,  author,  minReadingTime,  maxReadingTime,  sorting,
             page,  size,token, onlyMyCommunities);
        return Ok(posts);
    }    
    
    [HttpPost("{id:guid}/like")]
    [Authorize]
    public async Task<IActionResult> SetLike(Guid id)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        await _postService.AddLike(token,id);
        return Ok();
    }    
    [HttpDelete("{id:guid}/like")]
    [Authorize]
    public async Task<IActionResult> DeleteLike(Guid id)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        await _postService.DeleteLike(token,id);
        return Ok();
    }    
}