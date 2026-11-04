using System.ComponentModel.DataAnnotations.Schema;

namespace Skyway2.Entities;

public class Ticket
{
    public int TicketId { get; set; }
    public DateTime Date { get; set; }
    public int OrderId { get; set; }
    public virtual Order Order { get; set; }
    
   // public int FlightId { get; set; }
   // [ForeignKey("FlightId")] 
   // public virtual Flight Flight { get; set; }

    //public int PaymentId { get; set; }
  //  [ForeignKey("PaymentId")]
    //public virtual PaymentStatus PaymentStatus { get; set; }
   // public int PassengerId { get; set; }
    //[ForeignKey("PassengerId")]
    //public virtual Passenger Passenger { get; set; }
}