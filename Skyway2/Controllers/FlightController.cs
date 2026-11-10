using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skyway2.InterFaces;
using Skyway2.Dtos;

namespace Skyway2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightController(IFlightRepository _flightRepository) : ControllerBase
{
    [Authorize(Roles = "Admin,Passenger")]
    [HttpGet]
    public async Task<IActionResult> GetAllFlights()
    {
        var result = await _flightRepository.GetAllFlights();
        return StatusCode(result.statusCode, result);
    }
    [Authorize(Roles = "Admin,Passenger")]
    [HttpGet("{flightId}")]
    public async Task<IActionResult> GetFlightById(int flightId)
    {
        var result = await _flightRepository.GetFlightByFlightId(flightId);
        return StatusCode(result.statusCode, result);
    }
    [Authorize(Roles = "Admin,Passenger")]
    [HttpGet("Departure/{departureCityId}")]
    public async Task<IActionResult> GetByDepartureCity(int departureCityId)
    {
        var result = await _flightRepository.GetFlightByDepartureCityId(departureCityId);
        return StatusCode(result.statusCode, result);
    }
    [Authorize(Roles = "Admin,Passenger")]
    [HttpGet("Destination/{destinationCityId}")]
    public async Task<IActionResult> GetByDestinationCity(int destinationCityId)
    {
        var result = await _flightRepository.GetFlightByDestinationCityId(destinationCityId);
        return StatusCode(result.statusCode, result);
    }
    [Authorize(Roles = "Admin,Passenger")]
    [HttpGet("DateRange")]
    public async Task<IActionResult> GetByDateRange(DateTime startDate, DateTime endDate)
    {
        var result = await _flightRepository.GetFlightsByDateRange(startDate, endDate);
        return StatusCode(result.statusCode, result);
    }
    [Authorize(Roles = "Admin,Passenger")]
    [HttpGet("Search/Departure")]
    public async Task<IActionResult> SearchByDepartureCity(string searchText)
    {
        var result = await _flightRepository.SearchFlightsByDepartureCity(searchText);
        return StatusCode(result.statusCode, result);
    }
    [Authorize(Roles = "Admin,Passenger")]
    [HttpGet("Search/Destination")]
    public async Task<IActionResult> SearchByDestinationCity(string searchText)
    {
        var result = await _flightRepository.SearchFlightsByDestinationCity(searchText);
        return StatusCode(result.statusCode, result);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateFlight(CreateFlightDto flight)
    {
        var result = await _flightRepository.CreateFlight(flight);
        return StatusCode(result.statusCode, result);
    }
    [Authorize(Roles = "Admin")]
    [HttpPut("{flightId}")]
    public async Task<IActionResult> UpdateFlight(int flightId, UpdateflightDto flight)
    {
        var result = await _flightRepository.UpdateFlight(flightId, flight);
        return StatusCode(result.statusCode, result);
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{flightId}")]
    public async Task<IActionResult> DeleteFlight(int flightId)
    {
        var result = await _flightRepository.DeleteFlight(flightId);
        return StatusCode(result.statusCode, result);
    }
}