using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Skyway2.Entities;

public class City
{
    [Key]
    public int Id { get; set; }

    public string CityName { get; set; }

    public int CountryId { get; set; }

    [ForeignKey(nameof(CountryId))]
    public virtual Country Country { get; set; }
}