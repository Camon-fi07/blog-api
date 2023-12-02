using Microsoft.AspNetCore.Mvc;
using weblog_API.Models.Tags;
using weblog_API.Services.IServices;

namespace weblog_API.Controllers;

[Route("api/[controller]")]
public class TagController : Controller
{
    private readonly ITagsService _tagsService;

    public TagController(ITagsService tagsService)
    {
        _tagsService = tagsService;
    }
    
    [HttpGet()]
    public async Task<ActionResult<List<Tag>>> GetTags()
    {
        var tags = await _tagsService.GetTags();
        return Ok(tags);
    }
}