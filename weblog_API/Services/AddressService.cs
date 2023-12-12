using Microsoft.EntityFrameworkCore;
using weblog_API.Dto.Address;
using weblog_API.Enums;
using weblog_API.Middlewares;
using weblog_API.Models;
using weblog_API.Services.IServices;

namespace weblog_API.Services;

public class AddressService:IAddressService
{
    private readonly AddressDbContext _db;

    public AddressService(AddressDbContext db)
    {
        _db = db;
    }
    
    private async Task<SearchAddressDto?> GetAddressById(long? id)
    {
        AsAddrObj? addressObj;
        addressObj = await _db.AsAddrObjs.FirstOrDefaultAsync(a => a.Objectid == id);
        if (addressObj == null)
        {
            AsHouse? addressHouse;
            addressHouse = await _db.AsHouses.FirstOrDefaultAsync(a => a.Objectid == id);
            if (addressHouse == null) return null;
            var text = $"{addressHouse.Housenum}";
            if (addressHouse.Addnum1 != null) text += $" {AddressType.GetHouseType(addressHouse.Addtype1)} {addressHouse.Addnum1}";
            if (addressHouse.Addnum2 != null) text += $" {AddressType.GetHouseType(addressHouse.Addtype1)} {addressHouse.Addnum2}";
            return new SearchAddressDto()
            {
                Objectguid = addressHouse.Objectguid,
                Objectid = addressHouse.Objectid,
                Text = text,
                ObjectLevel = ObjectLevel.Building.ToString(),
                ObjectLevelText = AddressType.GetHouseType(addressHouse.Housetype)
            };
        }
        return new SearchAddressDto()
        {
            Objectguid = addressObj.Objectguid,
            Objectid = addressObj.Objectid,
            Text = $"{addressObj.Typename} {addressObj.Name}",
            ObjectLevel = addressObj.Level,
            ObjectLevelText = AddressType.GetAddressType(addressObj.Level)
        };
    }

    public async Task<bool> IsAddressAvailable(Guid id)
    {
        var isAddressObj = await _db.AsAddrObjs.AnyAsync(a => a.Objectguid == id);
        if (!isAddressObj)
        {
            var isAddressHouse = await _db.AsHouses.AnyAsync(a => a.Objectguid == id);
            return isAddressHouse;
        }
        return isAddressObj;
    }

    private async Task<long?> GetIdByGuid(Guid guid)
    {
        var id = (await _db.AsAddrObjs.FirstOrDefaultAsync(a => a.Objectguid == guid))?.Objectid;
        if (id == null) id = (await _db.AsHouses.FirstOrDefaultAsync(a => a.Objectguid == guid))?.Objectid;
        if (id == null) throw new CustomException("can't find address with this id",400);
        return id;
    }
    
    public async Task<List<SearchAddressDto>> Search(long parentObjectId, string? query)
    {
        var addressesHierarchies = _db.AsAdmHierarchies.Where(a => a.Parentobjid == parentObjectId).ToList();
        List<SearchAddressDto> resultAddresses = new List<SearchAddressDto>();
        foreach (var address in addressesHierarchies)
        {
            var searchAddress = await GetAddressById(address.Objectid);
            if (searchAddress == null) continue;
            if (searchAddress.Text.ToLower().Contains(query == null ? "" : query.ToLower())) resultAddresses.Add(searchAddress);
            if (resultAddresses.Count == 10) break;
        }

        return resultAddresses;
    }

    public async Task<List<SearchAddressDto>> AddressChain(Guid objectGuid)
    {
        var objectId = await GetIdByGuid(objectGuid);
        var ids = (await _db.AsAdmHierarchies.FirstOrDefaultAsync(a => a.Objectid == objectId))?.Path!.Split(".");
        List<SearchAddressDto> path = new List<SearchAddressDto>();
        foreach (var id in ids)
        {   
            path.Add(await GetAddressById(long.Parse(id)));
        }

        return path;
    }
}