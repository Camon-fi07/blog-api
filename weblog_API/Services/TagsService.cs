using Microsoft.EntityFrameworkCore;
using weblog_API.Data;
using weblog_API.Models.Tags;
using weblog_API.Services.IServices;

namespace weblog_API.Services;

public class TagsService:ITagsService
{
    private readonly AppDbContext _db;
    public TagsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Tag>> GetTags()
    {
        var tags = await _db.Tags.ToListAsync();
        return tags;
    }
}