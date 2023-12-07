using weblog_API.Data.Dto;

namespace weblog_API.Services.IServices;

public interface IAuthorService
{
    List<AuthorDto> GetAuthorList();
}