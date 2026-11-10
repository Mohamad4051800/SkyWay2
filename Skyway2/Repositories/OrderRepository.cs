using Microsoft.EntityFrameworkCore;
using Skyway2.Common;
using Skyway2.Dtos;
using Skyway2.Entities;
using Skyway2.InterFaces;

namespace Skyway2.Repositories;

public class OrderRepository(AppDbContext _order) : IOrderRepository
{
    public async Task<Response<int>> CreateOrder(string userId, CreateOrderDto orderDto)    
    {
        var response = new Response<int>();
        response.payload = 0;

        try
        {
            var passenger = await _order.Passengers
                .FirstOrDefaultAsync(p => p.ApplicationUserId == userId);

            if (passenger == null)
            {
                response.statusCode = 404;
                response.message = "مسافر یافت نشد";
                response.devMessage = "Passenger not found";
                response.messageLevel = MessageLevel.Admin;
                return response;
            }

            var flightExists = await _order.Flights
                .AnyAsync(f => f.FlightId == orderDto.FlightId);

            if (!flightExists)
            {
                response.statusCode = 404;
                response.message = "پرواز یافت نشد";
                response.devMessage = "Flight not found";
                response.messageLevel = MessageLevel.Admin;
                return response;
            }
            var orderExists = await _order.Orders
                .AnyAsync(o =>
                    o.PassengerId == passenger.PassengerId &&
                    o.FlightId == orderDto.FlightId);

            if (orderExists)
            {
                response.statusCode = 409;
                response.message = "شما قبلاً برای این پرواز سفارش ثبت کرده‌اید";
                response.devMessage = "Order already exists for this passenger and flight";
                response.messageLevel = MessageLevel.Admin;
                return response;
            }

            var order = new Order
            {
                PassengerId = passenger.PassengerId,
                FlightId = orderDto.FlightId,
                IsPaid = false
            };
            await _order.Orders.AddAsync(order);
            await _order.SaveChangesAsync();

            response.payload = order.OrderId;
            response.statusCode = 201;
            response.message = "سفارش با موفقیت ایجاد شد";
            response.devMessage = $"Order with id {order.OrderId} created successfully";
            response.title = "Create Order Success";
            response.messageLevel = MessageLevel.Admin;
        }
        catch (Exception ex)
        {
            response.statusCode = 500;
            response.message = "خطا در ایجاد سفارش";
            response.devMessage = ex.Message;
            response.messageLevel = MessageLevel.Admin;
        }

        return response;
    }

    public async Task<Response<bool>> ConfirmOrderPayment(ConfirmOrderPaymentDto dto)
    {
        var response = new Response<bool>();
        response.payload = false;

        try
        {
            await using var transaction = await _order.Database.BeginTransactionAsync();

            var order = await _order.Orders
                .Include(o => o.Flight)
                .Include(o => o.Ticket)
                .FirstOrDefaultAsync(o => o.OrderId == dto.OrderId);

            if (order == null)
            {
                response.statusCode = 404;
                response.message = "سفارش یافت نشد";
                response.devMessage = "Order not found";
                response.messageLevel = MessageLevel.Admin;
                return response;
            }

            if (order.IsPaid || order.Ticket != null)
            {
                response.statusCode = 409;
                response.message = "این سفارش قبلاً پرداخت یا بلیت آن صادر شده است";
                response.devMessage = "Order already paid or ticket already exists";
                response.messageLevel = MessageLevel.Admin;
                return response;
            }

            order.IsPaid = true;

            var ticket = new Ticket
            {
                OrderId = order.OrderId,
                Date = order.Flight.DateAndTimeOfFlight
            };

            await _order.Tickets.AddAsync(ticket);

            await _order.SaveChangesAsync();

            await transaction.CommitAsync();

            response.payload = true;
            response.statusCode = 200;
            response.message = "پرداخت تأیید شد و بلیت با موفقیت صادر شد";
            response.devMessage = "Payment confirmed and ticket created successfully";
            response.title = "Confirm Payment Success";
            response.messageLevel = MessageLevel.Admin;
        }
        catch (Exception ex)
        {
            response.statusCode = 500;
            response.message = "خطا در تأیید پرداخت";
            response.devMessage = ex.Message;
            response.messageLevel = MessageLevel.Admin;
        }

        return response;
    }
    public async Task<Response<List<OrderDto>>> GetAllOrders()
    {
        var response = new Response<List<OrderDto>>();

        try
        {
            response.payload = await _order.Orders
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    PassengerId = o.PassengerId,
                    FlightId = o.FlightId,
                    IsPaid = o.IsPaid,
                    FlightCode = o.Flight.FlightCode,
                    Date = o.Flight.DateAndTimeOfFlight,
                    Price = o.Flight.Price,
                    DepartureCity = o.Flight.DepartureCity.CityName,
                    DestinationCity = o.Flight.DestinationCity.CityName
                })
                .ToListAsync();

            response.statusCode = 200;
            response.message = "سفارش‌ها با موفقیت دریافت شدند";
            response.devMessage = "Orders retrieved successfully";
        }
        catch (Exception ex)
        {
            response.statusCode = 500;
            response.message = "خطا در دریافت سفارش‌ها";
            response.devMessage = ex.Message;
        }

        return response;
    }
    public async Task<Response<OrderDto>> GetOrderById(int orderId)
    {
        var response = new Response<OrderDto>();

        try
        {
            response.payload = await _order.Orders
                .Where(o => o.OrderId == orderId)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    PassengerId = o.PassengerId,
                    FlightId = o.FlightId,
                    IsPaid = o.IsPaid,
                    FlightCode = o.Flight.FlightCode,
                    Date = o.Flight.DateAndTimeOfFlight,
                    Price = o.Flight.Price,
                    DepartureCity = o.Flight.DepartureCity.CityName,
                    DestinationCity = o.Flight.DestinationCity.CityName
                })
                .FirstOrDefaultAsync();

            if (response.payload == null)
            {
                response.statusCode = 404;
                response.message = "سفارش یافت نشد";
                response.devMessage = "Order not found";
                return response;
            }

            response.statusCode = 200;
            response.message = "سفارش با موفقیت دریافت شد";
            response.devMessage = "Order retrieved successfully";
        }
        catch (Exception ex)
        {
            response.statusCode = 500;
            response.message = "خطا در دریافت سفارش";
            response.devMessage = ex.Message;
        }

        return response;
    }
    public async Task<Response<List<OrderDto>>> GetMyOrders(string userId)
    {
        var response = new Response<List<OrderDto>>();

        try
        {
            response.payload = await _order.Orders
                .Where(o => o.Passenger.ApplicationUserId == userId)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    PassengerId = o.PassengerId,
                    FlightId = o.FlightId,
                    IsPaid = o.IsPaid,
                    FlightCode = o.Flight.FlightCode,
                    Date = o.Flight.DateAndTimeOfFlight,
                    Price = o.Flight.Price,
                    DepartureCity = o.Flight.DepartureCity.CityName,
                    DestinationCity = o.Flight.DestinationCity.CityName
                })
                .ToListAsync();

            response.statusCode = 200;
            response.message = "سفارش‌های شما با موفقیت دریافت شدند";
            response.devMessage = "Passenger orders retrieved successfully";
        }
        catch (Exception ex)
        {
            response.statusCode = 500;
            response.message = "خطا در دریافت سفارش‌ها";
            response.devMessage = ex.Message;
        }

        return response;
    }
}