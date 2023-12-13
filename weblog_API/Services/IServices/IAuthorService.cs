using weblog_API.Dto.User;

namespace weblog_API.Services.IServices;

public interface IAuthorService
{
    List<AuthorDto> GetAuthorList();
}