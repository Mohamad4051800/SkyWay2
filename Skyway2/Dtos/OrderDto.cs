namespace Skyway2.Dtos;

public class OrderDto
{
    public int OrderId { get; set; }
    public int PassengerId { get; set; }
    public int FlightId { get; set; }
    public bool IsPaid { get; set; }

    public string FlightCode { get; set; }
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
    public string DepartureCity { get; set; }
    public string DestinationCity { get; set; }
}