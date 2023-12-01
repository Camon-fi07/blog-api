using Microsoft.AspNetCore.Mvc;
using weblog_API.Data.Dto;
using weblog_API.Enums;
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
    public async Task<IActionResult> CreateCommunity([FromBody] CreateCommunityDto communityInfo)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        try
        {
            await _communityService.CreateCommunity(communityInfo, token.Substring("Bearer ".Length));
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
    [HttpGet("")]
    public ActionResult<CommunityDto> GetCommunities()
    {
        try
        {
            var communityDtos = _communityService.GetCommunityList();
            return Ok(communityDtos);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CommunityFullDto>> GetCommunity(Guid id)
    {
        try
        {
            var community = await _communityService.GetCommunity(id);
            return Ok(community);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("{id:guid}/role")]
    public async Task<ActionResult<string?>> GetUserRole(Guid id)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        try
        {
            var role = await _communityService.GetUserRole(token.Substring("Bearer ".Length), id);
            return Ok(role);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("{id:guid}/subscribe")]
    public async Task<IActionResult> SubscribeUser(Guid id)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        try
        {
            await _communityService.SubscribeUser(token.Substring("Bearer ".Length), id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete("{id:guid}/unsubscribe")]
    public async Task<IActionResult> UnsubscribeUser(Guid id)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        try
        {
            await _communityService.UnsubscribeUser(token.Substring("Bearer ".Length), id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpDelete("{id:guid}/delete")]
    public async Task<IActionResult> DeleteCommunity(Guid id)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        try
        {
            await _communityService.DeleteCommunity(token.Substring("Bearer ".Length), id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}