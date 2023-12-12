using weblog_API.Dto.Address;

namespace weblog_API.Services.IServices;

public interface IAddressService
{
    Task<List<SearchAddressDto>> Search(long parentObjectId, string? query);
    
    Task<List<SearchAddressDto>> AddressChain(Guid objectGuid);

    public Task<bool> IsAddressAvailable(Guid id);
}