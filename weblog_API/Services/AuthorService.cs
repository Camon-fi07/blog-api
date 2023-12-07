using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using weblog_API.Data;
using weblog_API.Data.Dto;
using weblog_API.Mappers;
using weblog_API.Services.IServices;

namespace weblog_API.Services;

public class AuthorService:IAuthorService
{
    private readonly AppDbContext _db;

    public AuthorService(AppDbContext db)
    {
        _db = db;
    }

    public List<AuthorDto> GetAuthorList()
    {
        var users = _db.Users.Include(u => u.Posts).ThenInclude(p => p.UsersLiked).Where(u => u.Posts.Count > 0);
        return users.OrderBy(u => u.FullName).Select(u => UserMapper.UserToAuthorDto(u)).ToList();
    }
}