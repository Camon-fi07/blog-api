using weblog_API.Dto;

namespace weblog_API.Services.IServices;

public interface ITagsService
{
    public void CheckTags(List<Guid> tags);
    public List<TagDto> GetTags();
}