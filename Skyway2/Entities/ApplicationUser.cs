using Microsoft.AspNetCore.Identity;

namespace Skyway2.Entities;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public Passenger? Passenger { get; set; }
}