using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skyway2.InterFaces;

namespace Skyway2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketController(ITicketRepository _ticket) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet("GetTickets")]
    public async Task<IActionResult> GetTickets()
    {
        var response = await _ticket.GetTickets();
        return StatusCode(response.statusCode, response);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("GetTicketsByDate")]
    public async Task<IActionResult> GetTicketsByDate([FromQuery] DateOnly date)
    {
        var response = await _ticket.GetTicketsByDate(date);
        return StatusCode(response.statusCode, response);
    }

    [HttpGet("GetPassengerTickets")]
    [Authorize(Roles = "Passenger")]
    public async Task<IActionResult> GetPassengerTickets()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        var response = await _ticket.GetPassengerTickets(userId);

        return StatusCode(response.statusCode, response);
    }
}