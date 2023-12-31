using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using weblog_API.Data.Dto;
using weblog_API.Dto.Community;
using weblog_API.Dto.Post;
using weblog_API.Enums;
using weblog_API.Services.IServices;

namespace weblog_API.Controllers;

[Route("api/community")]
[ApiController]
public class CommunityController : Controller
{
    
    private readonly ICommunityService _communityService;
    private readonly IPostService _postService;
    public CommunityController(ICommunityService communityService, IPostService postService)
    {
        _communityService = communityService;
        _postService = postService;
    }
    
    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateCommunity([FromBody] CreateCommunityDto communityInfo)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        await _communityService.CreateCommunity(communityInfo, token);
        return Ok();
    }
    [HttpGet("")]
    public ActionResult<CommunityDto> GetCommunities()
    {
        var communityDtos = _communityService.GetCommunityList();
        return Ok(communityDtos);
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CommunityFullDto>> GetCommunity(Guid id)
    {
        var community = await _communityService.GetCommunity(id);
        return Ok(community);
    }
    
    [HttpPost("{id:guid}/post")]
    [Authorize]
    public async Task<ActionResult<Guid>> CreatePost([FromBody] CreatePostDto createPostDto, Guid id)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        var postId = await _postService.CreatePost(createPostDto, token, id);
        return Ok(postId);
    }
    
    [HttpGet("{id:guid}/post")]
    public async Task<ActionResult<PostPagedListDto>> GetPosts(Guid id,[FromQuery] List<Guid> tags, string? author, int? minReadingTime, int? maxReadingTime, PostSorting sorting,
        int page=1, int size=5)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        var posts = await _postService.GetCommunityPosts(id, tags,  author,  minReadingTime,  maxReadingTime,  sorting,
            page, size, token);
        return Ok(posts);
    }    
    
    
    [HttpGet("{id:guid}/role")]
    [Authorize]
    public async Task<ActionResult<RoleDto>> GetUserRole(Guid id)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        var role = await _communityService.GetUserRole(token, id);
        return Ok(role);
    }
    
    [HttpPost("{id:guid}/subscribe")]
    [Authorize]
    public async Task<IActionResult> SubscribeUser(Guid id)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        await _communityService.SubscribeUser(token, id);
        return Ok();
    }
    
    [HttpDelete("{id:guid}/unsubscribe")]
    [Authorize]
    public async Task<IActionResult> UnsubscribeUser(Guid id)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        await _communityService.UnsubscribeUser(token, id);
        return Ok();
    }
    [HttpDelete("{id:guid}/delete")]
    [Authorize]
    public async Task<IActionResult> DeleteCommunity(Guid id)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        await _communityService.DeleteCommunity(token, id);
        return Ok();
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<ActionResult<List<CommunityUserDto>>> GetUserCommunitites()
    {
        string token = HttpContext.Request.Headers["Authorization"];
        var communities = await _communityService.GetUserCommunityList(token);
        return Ok(communities);
    }
}