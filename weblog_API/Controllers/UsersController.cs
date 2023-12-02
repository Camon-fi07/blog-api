using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using weblog_API.Data.Dto;
using weblog_API.Models;
using weblog_API.Models.User;
using weblog_API.Services.IServices;

namespace weblog_API.Controllers;
[Route("api/Users")]
[ApiController]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<TokenModel>> Register([FromBody] UserRegister userRegister)
    {
        var token = await _userService.Registration(userRegister);
        return Ok(token);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenModel>> Login([FromBody] LoginCredentials loginCredentials)
    {
        var token = await _userService.Login(loginCredentials);
        return Ok(token);
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<UserDto>> Profile()
    {
        string token = HttpContext.Request.Headers["Authorization"];
        var user = await _userService.GetUser(token);
        return Ok(user);
        
    }

    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> EditProfile([FromBody] UserEdit userEdit)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        await _userService.Edit(userEdit, token);
        return Ok();
    }
    
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        string token = HttpContext.Request.Headers["Authorization"];
        await _userService.Logout(token);
        return Ok();
    }
}