namespace Skyway2.Entities;

public class Order
{
    public int OrderId { get; set; }

    public int PassengerId { get; set; }
    public virtual Passenger Passenger { get; set; }

    public int FlightId { get; set; }
    public virtual Flight Flight { get; set; }

    public bool IsPaid { get; set; } = false;

    public virtual Ticket? Ticket { get; set; }
}