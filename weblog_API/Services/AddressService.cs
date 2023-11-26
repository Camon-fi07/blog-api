using Microsoft.EntityFrameworkCore;
using weblog_API.Enums;
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
    
    private async Task<SearchAddress> getAddressById(long? id)
    {
        AsAddrObj? addressObj;
        addressObj = await _db.AsAddrObjs.FirstOrDefaultAsync(a => a.Objectid == id);
        if (addressObj == null)
        {
            AsHouse? addressHouse;
            addressHouse = await _db.AsHouses.FirstOrDefaultAsync(a => a.Objectid == id);
            if (addressHouse == null) throw new Exception("can't find address");
            return new SearchAddress()
            {
                Objectguid = addressHouse.Objectguid,
                Objectid = addressHouse.Objectid,
                Text = addressHouse.Housenum,
                ObjectLevel = ObjectLevel.Building.ToString(),
                ObjectLevelText = "Здание (сооружение)"
            };
        }
        return new SearchAddress()
        {
            Objectguid = addressObj.Objectguid,
            Objectid = addressObj.Objectid,
            Text = addressObj.Name,
            ObjectLevel = addressObj.Level,
            ObjectLevelText = addressObj.Typename
        };
    }

    private async Task<long?> getIdByGuid(Guid guid)
    {
        var id = (await _db.AsAddrObjs.FirstOrDefaultAsync(a => a.Objectguid == guid))?.Objectid;
        if (id == null) id = (await _db.AsHouses.FirstOrDefaultAsync(a => a.Objectguid == guid))?.Objectid;
        if (id == null) throw new Exception("can't find address with this id");
        return id;
    }
    
    public async Task<List<SearchAddress>> Search(long parentObjectId, string? query)
    {
        try
        {
            var addressesHierarchies = _db.AsAdmHierarchies.Where(a => a.Parentobjid == parentObjectId).ToList();
            List<SearchAddress> resultAddresses = new List<SearchAddress>();
            foreach (var address in addressesHierarchies)
            {
                var searchAddress = await getAddressById(address.Objectid);
                if (searchAddress.Text.ToLower().Contains(query == null ? "" : query.ToLower())) resultAddresses.Add(searchAddress);
                
            }

            return resultAddresses;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<SearchAddress>> AddressChain(Guid objectGuid)
    {
        try
        {
            var objectId = await getIdByGuid(objectGuid);
            var ids = (await _db.AsAdmHierarchies.FirstOrDefaultAsync(a => a.Objectid == objectId))?.Path!.Split(".");
            List<SearchAddress> path = new List<SearchAddress>();
            foreach (var id in ids)
            {   
                path.Add(await getAddressById(long.Parse(id)));
            }

            return path;
        }
        catch (Exception)
        {
            throw;
        }
    }
}