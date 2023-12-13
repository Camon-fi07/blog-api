using weblog_API.Data;
using weblog_API.Dto;
using weblog_API.Mappers;
using weblog_API.Middlewares;
using weblog_API.Services.IServices;

namespace weblog_API.Services;

public class TagsService:ITagsService
{
    private readonly AppDbContext _db;
    public TagsService(AppDbContext db)
    {
        _db = db;
    }

    public void CheckTags(List<Guid> tags)
    {
        foreach (var tagId in tags)
        {
            if (!_db.Tags.Any(t => t.Id == tagId)) throw new CustomException($"Can't find tag with {tagId} id", 400);
        }
    }
    
    public List<TagDto> GetTags()
    {
        var tags = _db.Tags.Select(t => TagMapper.TagToTagDto(t)).ToList();
        return tags;
    }
}