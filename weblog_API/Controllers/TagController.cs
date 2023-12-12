using Microsoft.AspNetCore.Mvc;
using weblog_API.Dto;
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
    public  ActionResult<List<TagDto>> GetTags()
    {
        var tags = _tagsService.GetTags();
        return Ok(tags);
    }
}