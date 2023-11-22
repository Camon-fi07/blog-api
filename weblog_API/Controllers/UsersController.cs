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
        bool isUserUnique = _userRepository.isUniqueUser(userRegister.Email);
        if (!isUserUnique) return BadRequest(new { message = "There is already a user with this email " });
        var token = await _userRepository.Registration(userRegister);
        return Ok(token);
    }

    [HttpPost("login")]
    public ActionResult<TokenResponseDto> Login([FromBody] LoginCredentials loginCredentials)
    {
        var token = _userRepository.Login(loginCredentials);
        if (token.Token.Length == 0) return BadRequest(new { message = "Invalid login or password" });
        return Ok(token);
    }

    [HttpGet("profile")]
    public ActionResult<UserDto> Profile()
    {
        string token = HttpContext.Request.Headers["Authorization"];
        if (token == null) return BadRequest();
        var user = _userRepository.GetUser(token);
        if (user == null) return BadRequest();
        return Ok(user);
    }
}