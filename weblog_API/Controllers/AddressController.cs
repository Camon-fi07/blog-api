using Microsoft.AspNetCore.Mvc;
using weblog_API.Dto.Address;
using weblog_API.Services.IServices;

namespace weblog_API.Controllers;

[Route("api/Address")]
[ApiController]
public class AddressController : Controller
{
    private readonly IAddressService _addressService;
    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }
    
    [HttpGet("search")]
    public async Task<ActionResult<SearchAddressDto>> Search(long parentObjectId, string? query)
    {
        try
        {
            var addresses = await _addressService.Search(parentObjectId, query);
            return Ok(addresses);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    [HttpGet("chain")]
    public async Task<ActionResult<SearchAddressDto>> Search(Guid objectGuid)
    {
        try
        {
            var path = await _addressService.AddressChain(objectGuid);
            return Ok(path);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
}