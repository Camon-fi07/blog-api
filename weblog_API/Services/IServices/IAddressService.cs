using weblog_API.Models;

namespace weblog_API.Services.IServices;

public interface IAddressService
{
    Task<List<SearchAddress>> Search(long parentObjectId, string? query);
    
    Task<List<SearchAddress>> AddressChain(Guid objectGuid);

    public Task<bool> IsAddressAvailable(Guid id);
}