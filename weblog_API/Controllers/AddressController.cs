using Microsoft.AspNetCore.Mvc;
using weblog_API.Models;
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
    public async Task<ActionResult<SearchAddress>> Search(long parentObjectId, string? query)
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
}