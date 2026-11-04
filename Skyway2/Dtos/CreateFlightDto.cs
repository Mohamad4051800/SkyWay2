namespace Skyway2.Dtos;

public class CreateFlightDto
{
    public string FlightCode { get; set; }

    public DateTime DateAndTimeOfFlight { get; set; }

    public decimal Price { get; set; }

    public int DepartureCityId { get; set; }

    public int DestinationCityId { get; set; }
}