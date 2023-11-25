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
        AsHouse? addressHouse;
        addressObj = await _db.AsAddrObjs.FirstOrDefaultAsync(a => a.Objectid == id);
        if (addressObj == null)
        {
            addressHouse = await _db.AsHouses.FirstOrDefaultAsync(a => a.Objectid == id);
            if (addressHouse == null) throw new Exception("can't find address");
            return new SearchAddress()
            {
                Objectguid = addressHouse.Objectguid,
                Objectid = addressHouse.Objectid,
                Text = addressHouse.Housenum,
                ObjectLevel = ObjectLevel.Building.ToString(),
                ObjectLevelText = "д"
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
    
    public async Task<List<SearchAddress>> Search(long parentObjectId, string? query)
    {
        try
        {
            var addressesHierarchies = _db.AsAdmHierarchies.Where(a => a.Parentobjid == parentObjectId).ToList();
            List<SearchAddress> resultAddresses = new List<SearchAddress>();
            foreach (var address in addressesHierarchies)
            {
                resultAddresses.Add(await getAddressById(address.Objectid));
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
        throw new NotImplementedException();
    }
}