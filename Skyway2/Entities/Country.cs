namespace Skyway2.Entities;

public class Country
{
    public int Id { get; set; }
    public string CountryName { get; set; }
    public virtual ICollection<City> Cities { get; set; }
   // public virtual ICollection<Flight> Flights { get; set; }

}