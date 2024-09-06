using Microsoft.AspNetCore.Mvc;
using TestAssigment_HK.Models;
using TestAssigment_HK.Models.DTO;
using TestAssigment_HK.Repositories;
using TestAssigment_HK.Services;

namespace TestAssigment_HK.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDTO userDto)
    {
        var result = await _userService.RegisterUserAsync(userDto);
        if (result.Success)
        {
            return Ok(result.Message);
        }

        return BadRequest(result.Message);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        var user = await _userService.AuthenticateUserAsync(loginDTO.EmailorUsername, loginDTO.Password);
        if (user == null)
        {
            return Unauthorized("Invalid login or password");
        }

        return Ok(user);
    }
}