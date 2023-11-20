using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> Register([FromBody] UserRegister userRegister)
    {
        bool isUserUnique = _userRepository.isUniqueUser(userRegister.Email);
        if (!isUserUnique) return BadRequest(new { message = "There is already a user with this email " });
        var token = await _userRepository.Registration(userRegister);
        return Ok(token);
    }
}