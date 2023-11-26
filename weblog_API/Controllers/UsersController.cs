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
        var isUserUnique = await _userService.isUniqueUser(userRegister.Email);
        if (!isUserUnique) return BadRequest(new { message = "There is already a user with this email " });
        var token = await _userService.Registration(userRegister);
        return Ok(token);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenModel>> Login([FromBody] LoginCredentials loginCredentials)
    {
        var token = await _userService.Login(loginCredentials);
        if (token.Token.Length == 0) return BadRequest(new { message = "Invalid login or password" });
        return Ok(token);
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<UserDto>> Profile()
    {
        string token = HttpContext.Request.Headers["Authorization"];
        if (token == null) return Unauthorized(new {message = "Invalid token"});
        try
        {
            var user = await _userService.GetUser(token.Substring("Bearer ".Length));
            return Ok(user);
        }
        catch(Exception err)
        {
            return BadRequest(new {message = err.Message});
        }
        
    }

    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> EditProfile([FromBody] UserEdit userEdit)
    {
        string token = HttpContext.Request.Headers["Authorization"];
        if (token == null) return Unauthorized(new {message = "Invalid token"});
        try
        {
            await _userService.Edit(userEdit, token.Substring("Bearer ".Length));
            return Ok();
        }
        catch(Exception err)
        {
            return BadRequest(new {message = err.Message});
        }
    }
    
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        string token = HttpContext.Request.Headers["Authorization"];
        if (token == null) return Unauthorized(new {message = "Invalid token"});
        try
        {
            await _userService.Logout(token.Substring("Bearer ".Length));
            return Ok();
        }
        catch(Exception err)
        {
            return BadRequest(new {message = err.Message});
        }
    }
}