using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skyway2.Dtos;
using Skyway2.InterFaces;

namespace Skyway2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthRepository _authRepository) : ControllerBase
{
    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterPassengerDto dto)
    {
        var result = await _authRepository.RegisterPassenger(dto);

        return StatusCode(result.statusCode, result);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _authRepository.Login(dto);

        return StatusCode(result.statusCode, result);
    }
    [Authorize]
    [HttpGet("Me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            Email = User.FindFirst(ClaimTypes.Email)?.Value,
            Role = User.FindFirst(ClaimTypes.Role)?.Value
        });
    }
    [Authorize(Roles = "Passenger")]
    [HttpGet("PassengerData")]
    public IActionResult PassengerData()
    {
        return Ok("Passenger Access Granted");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("AdminData")]
    public IActionResult AdminData()
    {
        return Ok("Admin Access Granted");
    }
}