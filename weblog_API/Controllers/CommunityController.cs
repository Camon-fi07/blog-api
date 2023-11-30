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
    public async Task<IActionResult> createCommunity([FromBody] CreateCommunityDto communityInfo)
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
    public ActionResult<CommunityDto> getCommunities()
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
    [HttpGet("community/{id:guid}")]
    public async Task<ActionResult<CommunityFullDto>> getCommunity(Guid id)
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
}