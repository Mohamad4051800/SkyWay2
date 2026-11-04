namespace Skyway2.Dtos;

public class TicketDto
{
    public int TicketId { get; set; }

    public string FlightCode { get; set; }

    public DateTime Date { get; set; }

    public decimal Price { get; set; }

    public string DepartureCity { get; set; }

    public string DestinationCity { get; set; }
}