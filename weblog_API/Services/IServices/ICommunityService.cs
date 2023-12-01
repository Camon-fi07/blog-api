using weblog_API.Data.Dto;

namespace weblog_API.Services.IServices;

public interface ICommunityService
{
    public Task CreateCommunity(CreateCommunityDto communityInfo,string token);

    public Task DeleteCommunity(string token, Guid communityId);
    
    public Task<CommunityFullDto> GetCommunity(Guid id);
    
    public List<CommunityDto> GetCommunityList();
    
    public Task<List<CommunityUserDto>> GetUserCommunityList(string token);

    public Task<string?> GetUserRole(string token, Guid communityId);

    public Task SubscribeUser(string token, Guid communityId);
    
    public Task UnsubscribeUser(string token, Guid communityId);
}