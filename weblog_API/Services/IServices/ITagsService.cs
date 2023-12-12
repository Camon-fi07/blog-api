using weblog_API.Dto;

namespace weblog_API.Services.IServices;

public interface ITagsService
{
    public List<TagDto> GetTags();
}