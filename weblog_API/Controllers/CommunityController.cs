using Microsoft.AspNetCore.Mvc;
using weblog_API.Data.Dto;
using weblog_API.Services.IServices;

namespace weblog_API.Controllers;

[Route("api/Community")]
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
            await _communityService.createCommunity(communityInfo, token.Substring("Bearer ".Length));
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
            var communityDtos = _communityService.getCommunityList();
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
            var community = await _communityService.getCommunity(id);
            return Ok(community);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
    [HttpGet("{id:guid}/role")]
    public async Task<ActionResult<CommunityFullDto>> GetUserRole(Guid id)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        try
        {
            var role = await _communityService.getUserRole(token.Substring("Bearer ".Length), id);
            return Ok(role);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("{id:guid}/subscribe")]
    public async Task<ActionResult<CommunityFullDto>> SubscribeUser(Guid id)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        try
        {
            await _communityService.subscribeUser(token.Substring("Bearer ".Length), id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete("{id:guid}/unsubscribe")]
    public async Task<ActionResult<CommunityFullDto>> UnsubscribeUser(Guid id)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        try
        {
            await _communityService.unsubscribeUser(token.Substring("Bearer ".Length), id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpDelete("{id:guid}/delete")]
    public async Task<ActionResult<CommunityFullDto>> DeleteCommunity(Guid id)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        try
        {
            await _communityService.deleteCommunity(token.Substring("Bearer ".Length), id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}