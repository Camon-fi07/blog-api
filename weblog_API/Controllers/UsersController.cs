using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using weblog_API.Data.Dto;
using weblog_API.Models.User;
using weblog_API.Repository.IRepository;

namespace weblog_API.Controllers;
[Route("api/Users")]
[ApiController]
public class UsersController : Controller
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<TokenResponseDto>> Register([FromBody] UserRegister userRegister)
    {
        var isUserUnique = await _userRepository.isUniqueUser(userRegister.Email);
        if (!isUserUnique) return BadRequest(new { message = "There is already a user with this email " });
        var token = await _userRepository.Registration(userRegister);
        return Ok(token);
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginCredentials loginCredentials)
    {
        var token = await _userRepository.Login(loginCredentials);
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
            var user = await _userRepository.GetUser(token.Substring("Bearer ".Length));
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
            await _userRepository.Edit(userEdit, token.Substring("Bearer ".Length));
            return Ok();
        }
        catch(Exception err)
        {
            return BadRequest(new {message = err.Message});
        }
    }
}