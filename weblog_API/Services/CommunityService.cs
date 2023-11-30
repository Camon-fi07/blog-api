using Microsoft.EntityFrameworkCore;
using weblog_API.Data;
using weblog_API.Data.Dto;
using weblog_API.Enums;
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

    public async Task createCommunity(CreateCommunityDto communityInfo, string token)
    {
        var creator = await _tokenService.GetUserByToken(token);
        var community = new Community()
        {
            Id = Guid.NewGuid(),
            CreateTime = DateTime.UtcNow,
            Description = communityInfo.Description,
            IsClosed = communityInfo.IsClosed,
            Name = communityInfo.Name,
            Subscribers = new List<UserCommunity>()
        };
        var userCommunity = new UserCommunity()
        {
            User = creator,
            CommunityId = community.Id,
            Community = community,
            UserRole = Role.Admin,
            UserId = creator.Id
        };
        community.Subscribers.Add(userCommunity);
        creator.Communities.Add(userCommunity);
        _db.Communities.Add(community);
        _db.UserCommunities.Add(userCommunity);
        await _db.SaveChangesAsync();
    }

    public List<CommunityDto> getCommunityList()
    {
        var communities = _db.Communities.Include(c => c.Subscribers).ToList();
        List<CommunityDto> communityDtos = new List<CommunityDto>();
        foreach (var community in communities)
        {
            communityDtos.Add(new CommunityDto()
            {
                Id = community.Id,
                Description = community.Description,
                IsClosed = community.IsClosed,
                Name = community.Name,
                CreateTime = community.CreateTime,
                SubscribersCount = community.Subscribers.Count
            });
        }

        return communityDtos;
    }

    public async Task<CommunityFullDto> getCommunity(Guid id)
    {
        var community = await _db.Communities.Include(c => c.Subscribers).ThenInclude(uc => uc.User).FirstOrDefaultAsync(c => c.Id==id);
        var admins = community.Subscribers.Where(c => c.UserRole == Role.Admin).ToList();
        List<UserDto> adminsDto = new List<UserDto>();
        foreach (var admin in admins)
        {
           adminsDto.Add(new UserDto()
           {
               Id = admin.User.Id,
               createTime = admin.User.CreateTime,
               Phone = admin.User.PhoneNumber,
               FullName = admin.User.FullName,
               Gender = admin.User.Gender,
               Email = admin.User.Email
           });
        }

        return new CommunityFullDto()
        {
            Id = community.Id,
            Description = community.Description,
            IsClosed = community.IsClosed,
            Name = community.Name,
            CreateTime = community.CreateTime,
            SubscribersCount = community.Subscribers.Count,
            Administrators = adminsDto
        };
    }

    public Task subscribeUser(string token, Guid communityId)
    {
        throw new NotImplementedException();
    }

    public Task unsubscribeUser(string token, Guid communityId)
    {
        throw new NotImplementedException();
    }

    public async Task<string> getUserRole(string token, Guid communityId)
    {
        var user = await _tokenService.GetUserByToken(token);
        var userCommunity = user.Communities.FirstOrDefault(c => c.CommunityId == communityId);
        if (userCommunity == null) throw new Exception("user is not a subscriber of this group");
        return Enum.GetName(typeof(Role), userCommunity.UserRole);
    }

    public Task<List<CommunityUserDto>> getUserCommunityList(string token)
    {
        throw new NotImplementedException();
    }
}