using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skyway2.InterFaces;
using Skyway2.Dtos;

namespace Skyway2.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class FlightController(IFlightRepository _flightRepository) : ControllerBase
{
    // Get All Flights
    [HttpGet]
    public async Task<IActionResult> GetAllFlights()
    {
        var result = await _flightRepository.GetAllFlights();
        return StatusCode(result.statusCode, result);
    }

    // Get Flight By Id
    [HttpGet("{flightId}")]
    public async Task<IActionResult> GetFlightById(int flightId)
    {
        var result = await _flightRepository.GetFlightByFlightId(flightId);
        return StatusCode(result.statusCode, result);
    }

    // Get Flights By Departure City
    [HttpGet("Departure/{departureCityId}")]
    public async Task<IActionResult> GetByDepartureCity(int departureCityId)
    {
        var result = await _flightRepository.GetFlightByDepartureCityId(departureCityId);
        return StatusCode(result.statusCode, result);
    }

    // Get Flights By Destination City
    [HttpGet("Destination/{destinationCityId}")]
    public async Task<IActionResult> GetByDestinationCity(int destinationCityId)
    {
        var result = await _flightRepository.GetFlightByDestinationCityId(destinationCityId);
        return StatusCode(result.statusCode, result);
    }

    // Get Flights By Date Range
    [HttpGet("DateRange")]
    public async Task<IActionResult> GetByDateRange(DateTime startDate, DateTime endDate)
    {
        var result = await _flightRepository.GetFlightsByDateRange(startDate, endDate);
        return StatusCode(result.statusCode, result);
    }

    // Search By Departure City
    [HttpGet("Search/Departure")]
    public async Task<IActionResult> SearchByDepartureCity(string searchText)
    {
        var result = await _flightRepository.SearchFlightsByDepartureCity(searchText);
        return StatusCode(result.statusCode, result);
    }

    // Search By Destination City
    [HttpGet("Search/Destination")]
    public async Task<IActionResult> SearchByDestinationCity(string searchText)
    {
        var result = await _flightRepository.SearchFlightsByDestinationCity(searchText);
        return StatusCode(result.statusCode, result);
    }

    // Create Flight
    [HttpPost]
    public async Task<IActionResult> CreateFlight(CreateFlightDto flight)
    {
        var result = await _flightRepository.CreateFlight(flight);
        return StatusCode(result.statusCode, result);
    }

    // Update Flight
    [HttpPut("{flightId}")]
    public async Task<IActionResult> UpdateFlight(int flightId, UpdateflightDto flight)
    {
        var result = await _flightRepository.UpdateFlight(flightId, flight);
        return StatusCode(result.statusCode, result);
    }

    // Delete Flight
    [HttpDelete("{flightId}")]
    public async Task<IActionResult> DeleteFlight(int flightId)
    {
        var result = await _flightRepository.DeleteFlight(flightId);
        return StatusCode(result.statusCode, result);
    }
}