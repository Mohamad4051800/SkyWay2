using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Skyway2.Entities;

namespace Skyway2.Common;

public class AppDbContext : IdentityDbContext<ApplicationUser>{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public AppDbContext()
    {
    }

    public DbSet<City> City { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Passenger> Passengers { get; set; }
    public DbSet<PaymentStatus> PaymentStatuses { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                @"Data Source=localhost;Initial Catalog=SkyWay2;Integrated Security=True;Trust Server Certificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Flight>()
            .HasOne(f => f.DepartureCity)
            .WithMany()
            .HasForeignKey(f => f.DepartureCityId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Flight>()
            .HasOne(f => f.DestinationCity)
            .WithMany()
            .HasForeignKey(f => f.DestinationCityId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Passenger)
            .WithMany()
            .HasForeignKey(o => o.PassengerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Flight)
            .WithMany()
            .HasForeignKey(o => o.FlightId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Ticket)
            .WithOne(t => t.Order)
            .HasForeignKey<Ticket>(t => t.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Flight>()
            .Property(f => f.Price)
            .HasPrecision(10, 2);
        modelBuilder.Entity<Passenger>()
            .HasOne(p => p.ApplicationUser)
            .WithOne(u => u.Passenger)
            .HasForeignKey<Passenger>(p => p.ApplicationUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}