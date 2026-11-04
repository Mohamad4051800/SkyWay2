using System.ComponentModel.DataAnnotations.Schema;
using Skyway2.Entities;

namespace Skyway2.Dtos;

public class CreateTicketDto
{
    public DateOnly Date { get; set; }
    public int FlightId { get; set; }
    [ForeignKey("FlightId")]

    public virtual Flight Flight { get; set; }

    public int PaymentId { get; set; }
    [ForeignKey("PaymentId")]
    public virtual PaymentStatus PaymentStatus { get; set; }
    public int PassengerId { get; set; }
    [ForeignKey("PassengerId")]
    public virtual Passenger Passenger { get; set; }
}