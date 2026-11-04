using System.ComponentModel.DataAnnotations;
using Skyway2.Entities;

namespace Skyway2.Dtos;

public class PassengerDto
{
    public int PassengerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string NationalCode { get; set; }
    public virtual ICollection<Ticket> Tickets { get; set; }
}