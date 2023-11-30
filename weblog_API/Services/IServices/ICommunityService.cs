using weblog_API.Data.Dto;

namespace weblog_API.Services.IServices;

public interface ICommunityService
{
    public Task createCommunity(CreateCommunityDto communityInfo,string token);

    public Task deleteCommunity(string token, Guid communityId);
    
    public Task<CommunityFullDto> getCommunity(Guid id);
    
    public List<CommunityDto> getCommunityList();
    
    public Task<List<CommunityUserDto>> getUserCommunityList(string token);

    public Task<string> getUserRole(string token, Guid communityId);

    public Task subscribeUser(string token, Guid communityId);
    
    public Task unsubscribeUser(string token, Guid communityId);
}