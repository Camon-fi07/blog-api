using weblog_API.Data.Dto;
using weblog_API.Dto.Community;
using weblog_API.Enums;
using weblog_API.Models.Community;

namespace weblog_API.Services.IServices;

public interface ICommunityService
{
    public Task<Community> GetCommunityById(Guid id);
    public Task CreateCommunity(CreateCommunityDto communityInfo,string token);

    public Task DeleteCommunity(string token, Guid communityId);
    
    public Task<CommunityFullDto> GetCommunity(Guid id);
    
    public List<CommunityDto> GetCommunityList();
    
    public Task<List<CommunityUserDto>> GetUserCommunityList(string token);

    public Task<RoleDto> GetUserRole(string token, Guid communityId);

    public Task SubscribeUser(string token, Guid communityId);
    
    public Task UnsubscribeUser(string token, Guid communityId);
}