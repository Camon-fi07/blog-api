using weblog_API.Data.Dto;
using weblog_API.Models.Tags;

namespace weblog_API.Services.IServices;

public interface ITagsService
{
    public List<TagDto> GetTags();
}