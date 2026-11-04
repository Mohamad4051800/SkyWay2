
using Microsoft.EntityFrameworkCore;
using Skyway2.Common;
using Skyway2.Dtos;
using Skyway2.Entities;
using Skyway2.InterFaces;

namespace Skyway2.Repositories;

public class FlightRepository(AppDbContext _flight) : IFlightRepository
{
    // Get All Flights
    public async Task<Response<List<FlightDto>>> GetAllFlights()
    {
        var response = new Response<List<FlightDto>>();
        response.payload = new List<FlightDto>();

        try
        {
            var flights = await _flight.Flights
                .Include(f => f.DepartureCity)
                .Include(f => f.DestinationCity)
                .ToListAsync();

            if (!flights.Any())
            {
                response.statusCode = 404;
                response.message = "پروازی یافت نشد";
                response.devMessage = "No Flights Found";
                response.title = "Get All Flights";
                response.messageLevel = MessageLevel.Admin;
                return response;
            }

            foreach (var f in flights)
            {
                response.payload.Add(MapFlight(f));
            }

            response.statusCode = 200;
            response.message = "عملیات با موفقیت انجام شد";
            response.title = "Get All Flights";
            response.messageLevel = MessageLevel.Admin;
        }
        catch (Exception ex)
        {
            response.payload = null;
            response.statusCode = 500;
            response.message = "خطا در دریافت لیست پروازها";
            response.devMessage = ex.Message;
            response.messageLevel = MessageLevel.Admin;
        }

        return response;
    }

    // Get Flight By Id
    public async Task<Response<List<FlightDto>>> GetFlightByFlightId(int FlightId)
    {
        var response = new Response<List<FlightDto>>();
        response.payload = new List<FlightDto>();

        try
        {
            var flight = await _flight.Flights
                .Include(f => f.DepartureCity)
                .Include(f => f.DestinationCity)
                .FirstOrDefaultAsync(f => f.FlightId == FlightId);

            if (flight == null)
            {
                response.payload = null;
                response.statusCode = 404;
                response.message = "پرواز یافت نشد";
                response.devMessage = $"Flight with ID {FlightId} not found";
                response.messageLevel = MessageLevel.Admin;
                return response;
            }

            response.payload.Add(MapFlight(flight));

            response.statusCode = 200;
            response.message = "عملیات با موفقیت انجام شد";
            response.title = "Get Flight By Id";
            response.messageLevel = MessageLevel.Admin;
        }
        catch (Exception ex)
        {
            response.payload = null;
            response.statusCode = 500;
            response.message = "خطا در دریافت اطلاعات پرواز";
            response.devMessage = ex.Message;
            response.messageLevel = MessageLevel.Admin;
        }

        return response;
    }

    // Get Flights By Departure City Id
    public async Task<Response<List<FlightDto>>> GetFlightByDepartureCityId(int departureCityId)
    {
        var response = new Response<List<FlightDto>>();
        response.payload = new List<FlightDto>();

        try
        {
            var flights = await _flight.Flights
                .Include(f => f.DepartureCity)
                .Include(f => f.DestinationCity)
                .Where(f => f.DepartureCityId == departureCityId)
                .ToListAsync();

            if (!flights.Any())
            {
                response.statusCode = 404;
                response.message = "پروازی از این شهر یافت نشد";
                response.devMessage = $"No flights found from city ID {departureCityId}";
                response.messageLevel = MessageLevel.Admin;
                return response;
            }

            foreach (var f in flights)
                response.payload.Add(MapFlight(f));

            response.statusCode = 200;
            response.message = "عملیات با موفقیت انجام شد";
            response.title = "Get Flights By Departure City";
            response.messageLevel = MessageLevel.Admin;
        }
        catch (Exception ex)
        {
            response.payload = null;
            response.statusCode = 500;
            response.message = "خطا در دریافت پروازها";
            response.devMessage = ex.Message;
            response.messageLevel = MessageLevel.Admin;
        }

        return response;
    }

    // Get Flights By Destination City Id
    public async Task<Response<List<FlightDto>>> GetFlightByDestinationCityId(int destinationCityId)
    {
        var response = new Response<List<FlightDto>>();
        response.payload = new List<FlightDto>();

        try
        {
            var flights = await _flight.Flights
                .Include(f => f.DepartureCity)
                .Include(f => f.DestinationCity)
                .Where(f => f.DestinationCityId == destinationCityId)
                .ToListAsync();

            if (!flights.Any())
            {
                response.statusCode = 404;
                response.message = "پروازی به این شهر یافت نشد";
                response.devMessage = $"No flights found to city ID {destinationCityId}";
                response.messageLevel = MessageLevel.Admin;
                return response;
            }

            foreach (var f in flights)
                response.payload.Add(MapFlight(f));

            response.statusCode = 200;
            response.message = "عملیات با موفقیت انجام شد";
            response.title = "Get Flights By Destination City";
            response.messageLevel = MessageLevel.Admin;
        }
        catch (Exception ex)
        {
            response.payload = null;
            response.statusCode = 500;
            response.message = "خطا در دریافت پروازها";
            response.devMessage = ex.Message;
            response.messageLevel = MessageLevel.Admin;
        }

        return response;
    }

    // Search Flights By Departure City Name
    public async Task<Response<List<FlightDto>>> SearchFlightsByDepartureCity(string searchText)
    {
        var response = new Response<List<FlightDto>>();
        response.payload = new List<FlightDto>();

        try
        {
            var flights = await _flight.Flights
                .Include(f => f.DepartureCity)
                .Include(f => f.DestinationCity)
                .Where(f => f.DepartureCity.CityName.Contains(searchText))
                .ToListAsync();

            if (!flights.Any())
            {
                response.statusCode = 404;
                response.message = "پروازی با این شهر مبدأ یافت نشد";
                response.messageLevel = MessageLevel.Admin;
                return response;
            }

            foreach (var f in flights)
                response.payload.Add(MapFlight(f));

            response.statusCode = 200;
            response.message = "عملیات با موفقیت انجام شد";
            response.title = "Search Flights By Departure City";
            response.messageLevel = MessageLevel.Admin;
        }
        catch (Exception ex)
        {
            response.payload = null;
            response.statusCode = 500;
            response.message = "خطا در جستجوی پروازها";
            response.devMessage = ex.Message;
            response.messageLevel = MessageLevel.Admin;
        }

        return response;
    }

    // Search Flights By Destination City Name
    public async Task<Response<List<FlightDto>>> SearchFlightsByDestinationCity(string searchText)
    {
        var response = new Response<List<FlightDto>>();
        response.payload = new List<FlightDto>();

        try
        {
            var flights = await _flight.Flights
                .Include(f => f.DepartureCity)
                .Include(f => f.DestinationCity)
                .Where(f => f.DestinationCity.CityName.Contains(searchText))
                .ToListAsync();

            if (!flights.Any())
            {
                response.statusCode = 404;
                response.message = "پروازی با این شهر مقصد یافت نشد";
                response.messageLevel = MessageLevel.Admin;
                return response;
            }

            foreach (var f in flights)
                response.payload.Add(MapFlight(f));

            response.statusCode = 200;
            response.message = "عملیات با موفقیت انجام شد";
            response.title = "Search Flights By Destination City";
            response.messageLevel = MessageLevel.Admin;
        }
        catch (Exception ex)
        {
            response.payload = null;
            response.statusCode = 500;
            response.message = "خطا در جستجوی پروازها";
            response.devMessage = ex.Message;
            response.messageLevel = MessageLevel.Admin;
        }

        return response;
    }

    // Get Flights By Date Range
    public async Task<Response<List<FlightDto>>> GetFlightsByDateRange(DateTime startDate, DateTime endDate)
    {
        var response = new Response<List<FlightDto>>();
        response.payload = new List<FlightDto>();

        try
        {
            if (startDate.Date > endDate.Date)
            {
                response.statusCode = 400;
                response.message = "تاریخ شروع نمی‌تواند بعد از تاریخ پایان باشد";
                response.devMessage = "Start date cannot be greater than end date";
                response.messageLevel = MessageLevel.Admin;
                return response;
            }

            var flights = await _flight.Flights
                .Include(f => f.DepartureCity)
                .Include(f => f.DestinationCity)
                .Where(f => f.DateAndTimeOfFlight >= startDate.Date &&
                            f.DateAndTimeOfFlight < endDate.Date.AddDays(1))
                .ToListAsync();

            if (!flights.Any())
            {
                response.statusCode = 404;
                response.message = "پروازی در این بازه زمانی یافت نشد";
                response.messageLevel = MessageLevel.Admin;
                return response;
            }

            foreach (var f in flights)
                response.payload.Add(MapFlight(f));

            response.statusCode = 200;
            response.message = "عملیات با موفقیت انجام شد";
            response.title = "Get Flights By Date Range";
            response.messageLevel = MessageLevel.Admin;
        }
        catch (Exception ex)
        {
            response.payload = null;
            response.statusCode = 500;
            response.message = "خطا در فیلتر پروازها";
            response.devMessage = ex.Message;
            response.messageLevel = MessageLevel.Admin;
        }

        return response;
    }

    // Create Flight
    public async Task<Response<int>> CreateFlight(CreateFlightDto flight)
    {
        try
        {
            var citiesExist = await _flight.City.AnyAsync(c =>
                c.Id == flight.DepartureCityId);

            var destinationExists = await _flight.City.AnyAsync(c =>
                c.Id == flight.DestinationCityId);

            if (!citiesExist || !destinationExists)
            {
                return new Response<int>
                {
                    payload = 0,
                    statusCode = 404,
                    message = "شهر مبدأ یا مقصد یافت نشد",
                    devMessage = "Departure or destination city not found",
                    title = "City Not Found",
                    messageLevel = MessageLevel.Admin
                };
            }

            var newFlight = new Flight
            {
                FlightCode = flight.FlightCode,
                DateAndTimeOfFlight = flight.DateAndTimeOfFlight,
                Price = flight.Price,
                DepartureCityId = flight.DepartureCityId,
                DestinationCityId = flight.DestinationCityId
            };

            _flight.Flights.Add(newFlight);
            await _flight.SaveChangesAsync();

            return new Response<int>
            {
                payload = newFlight.FlightId,
                statusCode = 201,
                message = "پرواز با موفقیت ایجاد شد",
                devMessage = $"Flight with ID {newFlight.FlightId} was created",
                title = "Create Flight Success",
                messageLevel = MessageLevel.Admin
            };
        }
        catch (Exception ex)
        {
            return new Response<int>
            {
                payload = 0,
                statusCode = 500,
                message = "خطا در ایجاد پرواز",
                devMessage = ex.Message,
                messageLevel = MessageLevel.Admin
            };
        }
    }

    // Update Flight
    public async Task<Response<bool>> UpdateFlight(int flightId, UpdateflightDto flight)
    {
        var response = new Response<bool>();
        response.payload = false;

        try
        {
            var existingFlight = await _flight.Flights
                .FirstOrDefaultAsync(f => f.FlightId == flightId);

            if (existingFlight == null)
            {
                response.statusCode = 404;
                response.message = "پرواز یافت نشد";
                response.devMessage = $"Flight with ID {flightId} not found";
                response.messageLevel = MessageLevel.Admin;
                return response;
            }

            existingFlight.DateAndTimeOfFlight = flight.DateAndTimeOfFlight;
            existingFlight.Price = flight.Price;

            await _flight.SaveChangesAsync();

            response.payload = true;
            response.statusCode = 200;
            response.message = "اطلاعات پرواز با موفقیت بروزرسانی شد";
            response.devMessage = "Flight updated successfully";
            response.title = "Update Flight Success";
            response.messageLevel = MessageLevel.Admin;
        }
        catch (Exception ex)
        {
            response.statusCode = 500;
            response.message = "خطا در بروزرسانی پرواز";
            response.devMessage = ex.Message;
            response.messageLevel = MessageLevel.Admin;
        }

        return response;
    }

    // Delete Flight
    public async Task<Response<bool>> DeleteFlight(int flightId)
    {
        var response = new Response<bool>();
        response.payload = false;

        try
        {
            var flight = await _flight.Flights
                .FirstOrDefaultAsync(f => f.FlightId == flightId);

            if (flight == null)
            {
                response.statusCode = 404;
                response.message = "پرواز یافت نشد";
                response.devMessage = $"Flight with ID {flightId} not found";
                response.messageLevel = MessageLevel.Admin;
                return response;
            }

            _flight.Flights.Remove(flight);
            await _flight.SaveChangesAsync();

            response.payload = true;
            response.statusCode = 200;
            response.message = "پرواز با موفقیت حذف شد";
            response.devMessage = "Flight deleted successfully";
            response.title = "Delete Flight Success";
            response.messageLevel = MessageLevel.Admin;
        }
        catch (Exception ex)
        {
            response.statusCode = 500;
            response.message = "خطا در حذف پرواز";
            response.devMessage = ex.Message;
            response.messageLevel = MessageLevel.Admin;
        }

        return response;
    }

    // Map Flight Entity To DTO
    private static FlightDto MapFlight(Flight f)
    {
        return new FlightDto
        {
            FlightId = f.FlightId,
            FlightCode = f.FlightCode,
            DateAndTimeOfFlight = f.DateAndTimeOfFlight,
            Price = f.Price,
            DepartureCityId = f.DepartureCityId,
            DepartureCity = f.DepartureCity,
            DestinationCityId = f.DestinationCityId,
            DestinationCity = f.DestinationCity
        };
    }
}
