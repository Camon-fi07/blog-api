using weblog_API.Models.Tags;

namespace weblog_API.Services.IServices;

public interface ITagsService
{
    public Task<List<Tag>> GetTags();
}