using System.ComponentModel.DataAnnotations;

namespace Skyway2.Entities;

public class Passenger
{
    [Key]
    public int PassengerId { get; set; }
    //[Required]
    //public string FirstName { get; set; }
    //[Required]
    //public string LastName { get; set; }
    //[Required]
    //public string Email { get; set; }
    //[Required]
    //public string PhoneNumber { get; set; }
    [Required]
    public DateOnly DateOfBirth { get; set; }
    [Required]
    public string NationalCode { get; set; } = null!;
    public string ApplicationUserId { get; set; } = null!;
    public ApplicationUser ApplicationUser { get; set; } = null!;
   // [Required]
    //public virtual ICollection<Ticket> Tickets { get; set; }
   // public virtual ICollection<Passenger> Passengers { get; set; }
    
}