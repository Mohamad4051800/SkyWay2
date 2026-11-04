using System.ComponentModel.DataAnnotations;

namespace Skyway2.Dtos;

public class CreatePassengerDto
{
    [Required]
    public DateOnly DateOfBirth { get; set; }

    [Required]
    public string NationalCode { get; set; }
}