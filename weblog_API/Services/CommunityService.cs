using Microsoft.EntityFrameworkCore;
using weblog_API.Data;
using weblog_API.Dto.Community;
using weblog_API.Dto.User;
using weblog_API.Enums;
using weblog_API.Mappers;
using weblog_API.Middlewares;
using weblog_API.Models.Community;
using weblog_API.Services.IServices;

namespace weblog_API.Services;

public class CommunityService:ICommunityService
{
    private readonly AppDbContext _db;
    private readonly IUserService _userService;
    public CommunityService(AppDbContext db, IUserService userService)
    {
        _db = db;
        _userService = userService;
    }

    public async Task<Community> GetCommunityById(Guid id)
    {
        var community = await _db.Communities.Include(c => c.Subscribers).ThenInclude(uc => uc.User)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (community == null) throw new CustomException("Invalid id", 400);
        return community;
    }
    
    public async Task CreateCommunity(CreateCommunityDto communityInfo, string token)
    {
        var creator = await _userService.GetUserByToken(token);
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
        var user = await _userService.GetUserByToken(token);
        var userCommunity = user.Communities.FirstOrDefault(uc => uc.CommunityId == communityId);
        if (userCommunity == null || userCommunity.UserRole != Role.Admin) throw new CustomException("You don't have rights", 403);
        var community = userCommunity.Community;
        var posts = community.Posts;
        foreach (var post in posts)
        {
            _db.Posts.Remove(post);
            _db.Entry(post).State = EntityState.Deleted;
        }
        community.Subscribers.Clear();
        _db.Communities.Remove(community);
        await _db.SaveChangesAsync();
    }

    public List<CommunityDto> GetCommunityList()
    {
        var communities = _db.Communities.Include(c => c.Subscribers)
            .Select(c => CommunityMapper.CommunityToCommunityDto(c)).ToList();
        return communities;
    }

    public async Task<CommunityFullDto> GetCommunity(Guid id)
    {
        var community = await GetCommunityById(id);
        var admins = community.Subscribers.Where(c => c.UserRole == Role.Admin).Select(a => new UserDto()
        {
            Id = a.User.Id,
            CreateTime = a.User.CreateTime,
            PhoneNumber = a.User.PhoneNumber,
            FullName = a.User.FullName,
            Gender = a.User.Gender,
            Email = a.User.Email
        }).ToList();

        return CommunityMapper.CommunityToCommunityFullDto(community, admins);
    }

    public async Task SubscribeUser(string token, Guid communityId)
    {
        var user = await _userService.GetUserByToken(token);
        var community = await GetCommunityById(communityId);
        if (community.Subscribers.Any(s => s.UserId == user.Id)) throw new CustomException("User is already a subscriber", 400);
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
        var user = await _userService.GetUserByToken(token);
        var community = await GetCommunityById(communityId);
        var userCommunity = community.Subscribers.FirstOrDefault(uc => uc.UserId == user.Id);
        if (userCommunity == null) throw new CustomException("User is not a subscriber of this group", 403);
        var admins = community.Subscribers.Where(uc => uc.UserRole == Role.Admin).ToList();
        if (community.Subscribers.Count == 1)
        {
            await DeleteCommunity(token, communityId);
        }
        else
        {
            if (userCommunity.UserRole == Role.Admin && admins.Count == 1)
            {
                var randomUser = community.Subscribers.FirstOrDefault(u => u.UserRole == Role.Subscriber)!;
                randomUser.UserRole = Role.Admin;
            }
            community.Subscribers.Remove(userCommunity);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<RoleDto> GetUserRole(string token, Guid communityId)
    {
        var user = await _userService.GetUserByToken(token);
        var userCommunity = user.Communities.FirstOrDefault(c => c.CommunityId == communityId);
        return new RoleDto() { Role = userCommunity?.UserRole };
    }

    public async Task<List<CommunityUserDto>> GetUserCommunityList(string token)
    {
        var user = await _userService.GetUserByToken(token);
        var communities = user.Communities.OrderBy(uc => uc.UserRole)
            .Select(c => CommunityMapper.UserCommunityToCommunityUserDto(c)).ToList();

        return communities;
    }
}