using Microsoft.EntityFrameworkCore;
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

    private async Task<AsAddrObj> getAddressById(long? id)
    {
        var address = await _db.AsAddrObjs.FirstOrDefaultAsync(a => a.Objectid == id);
        if (address == null) throw new Exception("can't find address");
        return address;
    }
    
    public async Task<List<SearchAddress>> Search(long parentObjectId, string? query)
    {
        try
        {
            var addresseHierarchies = _db.AsAdmHierarchies.Where(a => a.Parentobjid == parentObjectId).ToList();
            List<AsAddrObj> addresses = new List<AsAddrObj>();
            foreach (var address in addresseHierarchies)
            { 
                addresses.Add(await getAddressById(address.Objectid));
            }
            if(query!= null) addresses = addresses.Where(a => a.Name.ToLower().Contains(query.ToLower())).ToList();
            
            List<SearchAddress> resultAddresses = new List<SearchAddress>();
            foreach (var address in addresses)
            {
                resultAddresses.Add(new SearchAddress()
                {
                    Objectguid = address.Objectguid,
                    Objectid = address.Objectid,
                    Text = address.Name,
                    ObjectLevel = address.Level,
                    ObjectLevelText = address.Typename
                });
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