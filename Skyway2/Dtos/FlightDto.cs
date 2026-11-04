using System.ComponentModel.DataAnnotations.Schema;
using Skyway2.Entities;

namespace Skyway2.Dtos;

public class FlightDto
{
    public int FlightId { get; set; }

    public string FlightCode { get; set; }

    public DateTime DateAndTimeOfFlight { get; set; }

    public decimal Price { get; set; }

    public int DepartureCityId { get; set; }

    [ForeignKey(nameof(DepartureCityId))]
    public virtual City DepartureCity { get; set; }

    public int DestinationCityId { get; set; }

    [ForeignKey(nameof(DestinationCityId))]
    public virtual City DestinationCity { get; set; }
}