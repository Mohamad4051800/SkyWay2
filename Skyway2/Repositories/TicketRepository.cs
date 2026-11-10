using Microsoft.EntityFrameworkCore;
using Skyway2.Common;
using Skyway2.Dtos;
using Skyway2.InterFaces;

namespace Skyway2.Repositories;

public class TicketRepository(AppDbContext _ticket) : ITicketRepository
{
    public async Task<Response<List<TicketDto>>> GetTickets()
    {
        var response = new Response<List<TicketDto>>();

        try
        {
            var tickets = await _ticket.Tickets
                .Select(t => new TicketDto
                {
                    TicketId = t.TicketId,
                    FlightCode = t.Order.Flight.FlightCode,
                    Date = t.Order.Flight.DateAndTimeOfFlight,
                    Price = t.Order.Flight.Price,
                    DepartureCity = t.Order.Flight.DepartureCity.CityName,
                    DestinationCity = t.Order.Flight.DestinationCity.CityName
                })
                .ToListAsync();

            response.payload = tickets;
            response.statusCode = 200;
            response.message = "لیست بلیت‌ها با موفقیت دریافت شد";
            response.devMessage = "Tickets retrieved successfully";
            response.messageLevel = MessageLevel.Admin;
        }
        catch (Exception ex)
        {
            response.statusCode = 500;
            response.message = "خطا در دریافت بلیت‌ها";
            response.devMessage = ex.Message;
            response.messageLevel = MessageLevel.Admin;
        }

        return response;
    }

    public async Task<Response<List<TicketDto>>> GetTicketsByDate(DateOnly date)
    {
        var response = new Response<List<TicketDto>>();

        try
        {
            var tickets = await _ticket.Tickets
                .Where(t => DateOnly.FromDateTime(t.Order.Flight.DateAndTimeOfFlight) == date)
                .Select(t => new TicketDto
                {
                    TicketId = t.TicketId,
                    FlightCode = t.Order.Flight.FlightCode,
                    Date = t.Order.Flight.DateAndTimeOfFlight,
                    Price = t.Order.Flight.Price,
                    DepartureCity = t.Order.Flight.DepartureCity.CityName,
                    DestinationCity = t.Order.Flight.DestinationCity.CityName
                })
                .ToListAsync();

            response.payload = tickets;
            response.statusCode = 200;
            response.message = "بلیت‌های تاریخ موردنظر دریافت شدند";
            response.devMessage = "Tickets retrieved by date successfully";
            response.messageLevel = MessageLevel.Admin;
        }
        catch (Exception ex)
        {
            response.statusCode = 500;
            response.message = "خطا در دریافت بلیت‌ها بر اساس تاریخ";
            response.devMessage = ex.Message;
            response.messageLevel = MessageLevel.Admin;
        }

        return response;
    }

    public async Task<Response<List<TicketDto>>> GetPassengerTickets(string userId)
    {
        var response = new Response<List<TicketDto>>();

        try
        {
            var tickets = await _ticket.Tickets
                .Where(t => t.Order.Passenger.ApplicationUserId== userId)
                .Select(t => new TicketDto
                {
                    TicketId = t.TicketId,
                    FlightCode = t.Order.Flight.FlightCode,
                    Date = t.Order.Flight.DateAndTimeOfFlight,
                    Price = t.Order.Flight.Price,
                    DepartureCity = t.Order.Flight.DepartureCity.CityName,
                    DestinationCity = t.Order.Flight.DestinationCity.CityName
                })
                .ToListAsync();

            response.payload = tickets;
            response.statusCode = 200;
            response.message = "بلیت‌های مسافر با موفقیت دریافت شدند";
            response.devMessage = "Passenger tickets retrieved successfully";
            response.messageLevel = MessageLevel.Admin;
        }
        catch (Exception ex)
        {
            response.statusCode = 500;
            response.message = "خطا در دریافت بلیت‌های مسافر";
            response.devMessage = ex.Message;
            response.messageLevel = MessageLevel.Admin;
        }

        return response;
    }
}