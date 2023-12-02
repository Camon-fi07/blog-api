using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using weblog_API.Data.Dto;
using weblog_API.Services.IServices;

namespace weblog_API.Controllers;

[Route("api/community")]
[ApiController]
public class CommunityController : Controller
{
    
    private readonly ICommunityService _communityService;
    public CommunityController(ICommunityService communityService)
    {
        _communityService = communityService;
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
    
    [HttpGet("{id:guid}/role")]
    [Authorize]
    public async Task<ActionResult<string?>> GetUserRole(Guid id)
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