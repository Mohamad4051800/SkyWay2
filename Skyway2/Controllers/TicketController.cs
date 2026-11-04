using Microsoft.AspNetCore.Mvc;
using Skyway2.InterFaces;

namespace Skyway2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketController(ITicketRepository _ticket) : ControllerBase
{
    [HttpGet("GetTickets")]
    public async Task<IActionResult> GetTickets()
    {
        var response = await _ticket.GetTickets();
        return StatusCode(response.statusCode, response);
    }

    [HttpGet("GetTicketsByDate")]
    public async Task<IActionResult> GetTicketsByDate([FromQuery] DateOnly date)
    {
        var response = await _ticket.GetTicketsByDate(date);
        return StatusCode(response.statusCode, response);
    }

    [HttpGet("GetPassengerTickets/{passengerId}")]
    public async Task<IActionResult> GetPassengerTickets(int passengerId)
    {
        var response = await _ticket.GetPassengerTickets(passengerId);
        return StatusCode(response.statusCode, response);
    }
}