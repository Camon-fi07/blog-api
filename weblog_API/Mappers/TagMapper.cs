using weblog_API.Dto;
using weblog_API.Models.Tag;

namespace weblog_API.Mappers;

public static class TagMapper
{
    public static TagDto TagToTagDto(Tag tag)
    {
        var tagDto = new TagDto()
        {
            Id = tag.Id,
            CreateTime = tag.CreateTime,
            Name = tag.Name
        };
        return tagDto;
    }
}