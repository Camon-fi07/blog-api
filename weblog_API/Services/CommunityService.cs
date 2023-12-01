using Microsoft.EntityFrameworkCore;
using weblog_API.Data;
using weblog_API.Data.Dto;
using weblog_API.Enums;
using weblog_API.Models.Community;
using weblog_API.Services.IServices;

namespace weblog_API.Services;

public class CommunityService:ICommunityService
{
    private readonly AppDbContext _db;
    private ITokenService _tokenService;
    public CommunityService(AppDbContext db, ITokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }

    private async Task<Community> GetCommunityById(Guid Id)
    {
        try
        {
            var community = await _db.Communities.Include(c => c.Subscribers).ThenInclude(uc => uc.User)
                .FirstOrDefaultAsync(c => c.Id == Id);
            if (community == null) throw new Exception("invalid id");
            return community;
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    public async Task CreateCommunity(CreateCommunityDto communityInfo, string token)
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

    public async Task DeleteCommunity(string token, Guid communityId)
    {
        var user = await _tokenService.GetUserByToken(token);
        var userCommunity = user.Communities.FirstOrDefault(uc => uc.CommunityId == communityId);
        if (userCommunity == null || userCommunity.UserRole != Role.Admin) throw new Exception("you don't have rights");
        var userCommunitiesList = _db.UserCommunities.ToList();
        var community = userCommunity.Community;
        foreach (var subscriberCommunity in community.Subscribers)
        {
            var subscriber = subscriberCommunity.User;
            user.Communities.Remove(subscriberCommunity);
            userCommunitiesList.Remove(subscriberCommunity);
        }
        _db.Communities.Remove(community);
        await _db.SaveChangesAsync();
    }

    public List<CommunityDto> GetCommunityList()
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

    public async Task<CommunityFullDto> GetCommunity(Guid id)
    {
        var community = await _db.Communities.Include(c => c.Subscribers).ThenInclude(uc => uc.User).FirstOrDefaultAsync(c => c.Id==id);
        if (community == null) throw new Exception("there is not community with this Id");
        var admins = community.Subscribers.Where(c => c.UserRole == Role.Admin).Select(a => new UserDto()
        {
            Id = a.User.Id,
            createTime = a.User.CreateTime,
            Phone = a.User.PhoneNumber,
            FullName = a.User.FullName,
            Gender = a.User.Gender,
            Email = a.User.Email
        }).ToList();

        return new CommunityFullDto()
        {
            Id = community.Id,
            Description = community.Description,
            IsClosed = community.IsClosed,
            Name = community.Name,
            CreateTime = community.CreateTime,
            SubscribersCount = community.Subscribers.Count,
            Administrators = admins
        };
    }

    public async Task SubscribeUser(string token, Guid communityId)
    {
        var user = await _tokenService.GetUserByToken(token);
        var community = await GetCommunityById(communityId);
        UserCommunity userCommunity = new UserCommunity()
        {
            User = user,
            CommunityId = communityId,
            Community = community,
            UserRole = Role.Subscriber,
            UserId = user.Id
        };
        community.Subscribers.Add(userCommunity);
        user.Communities.Add(userCommunity);
        _db.UserCommunities.Add(userCommunity);
        await _db.SaveChangesAsync();
    }

    public async Task UnsubscribeUser(string token, Guid communityId)
    {
        var user = await _tokenService.GetUserByToken(token);
        var community = await GetCommunityById(communityId);
        var userCommunity = community.Subscribers.FirstOrDefault(uc => uc.UserId == user.Id);
        if (userCommunity == null) throw new Exception("user is not a subscriber of this group");
        var admins = community.Subscribers.Where(uc => uc.UserRole == Role.Admin).ToList();
        if (admins.Count == 1 && userCommunity.UserRole == Role.Admin){ await DeleteCommunity(token, communityId);}
        else
        {
            community.Subscribers.Remove(userCommunity);
            user.Communities.Remove(userCommunity);
            _db.UserCommunities.ToList().Remove(userCommunity);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<string?> GetUserRole(string token, Guid communityId)
    {
        var user = await _tokenService.GetUserByToken(token);
        var userCommunity = user.Communities.FirstOrDefault(c => c.CommunityId == communityId);
        return userCommunity == null ? null : Enum.GetName(typeof(Role), userCommunity.UserRole);
    }

    public async Task<List<CommunityUserDto>> GetUserCommunityList(string token)
    {
        var user = await _tokenService.GetUserByToken(token);
        var communities = user.Communities.OrderBy(uc => uc.UserRole).Select(c => new CommunityUserDto()
        {
            CommunityId = c.CommunityId,
            UserId = c.UserId,
            Role = Enum.GetName(typeof(Role), c.UserRole)
        }).ToList();

        return communities;
    }
}