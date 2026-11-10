using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Skyway2.InterFaces;
using Skyway2.Dtos;

namespace Skyway2.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Passenger")]
public class PassengerController(
    IPassengerRepository passengerRepository,
    ITicketRepository _ticketRepository) : ControllerBase
{
    [HttpGet("MyProfile")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        var result = await passengerRepository.GetMyProfile(userId);

        return StatusCode(result.statusCode, result);
    }

    [HttpPut("MyProfile")]
    public async Task<IActionResult> UpdateMyProfile(
        UpdatePassengerDto passengerDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        var result = await passengerRepository.UpdateMyProfile(
            userId, passengerDto);

        return StatusCode(result.statusCode, result);
    }
    [HttpGet("MyTickets")]
    public async Task<IActionResult> GetMyTickets()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        var result = await _ticketRepository.GetPassengerTickets(userId);

        return StatusCode(result.statusCode, result);
    }
}