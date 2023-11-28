using weblog_API.Data;
using weblog_API.Data.Dto;
using weblog_API.Models.Community;

namespace weblog_API.Services.IServices;

public class CommunityService:ICommunityService
{
    private readonly AppDbContext _db;
    private ITokenService _tokenService;
    public CommunityService(AppDbContext db, ITokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }

    public Task createCommunity(string token)
    {
        throw new NotImplementedException();
    }

    public List<CommunityDto> getCommunityList()
    {
        throw new NotImplementedException();
    }

    public Task<CommunityFullDto> getCommunity(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task subscribeUser(string token, Guid communityId)
    {
        throw new NotImplementedException();
    }

    public Task unsubscribeUser(string token, Guid communityId)
    {
        throw new NotImplementedException();
    }

    public Task<string> getUserRole(string token, Guid communityId)
    {
        throw new NotImplementedException();
    }

    public Task<List<CommunityUserDto>> getUserCommunityList(string token)
    {
        throw new NotImplementedException();
    }
}