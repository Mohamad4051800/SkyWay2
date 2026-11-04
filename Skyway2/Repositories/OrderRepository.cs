using Microsoft.EntityFrameworkCore;
using Skyway2.Common;
using Skyway2.Dtos;
using Skyway2.Entities;
using Skyway2.InterFaces;

namespace Skyway2.Repositories;

public class OrderRepository(AppDbContext _order) : IOrderRepository
{
    public async Task<Response<int>> CreateOrder(CreateOrderDto orderDto)
    {
        var response = new Response<int>();
        response.payload = 0;

        try
        {
            var passengerExists = await _order.Passengers
                .AnyAsync(p => p.PassengerId == orderDto.PassengerId);

            if (!passengerExists)
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

            var order = new Order
            {
                PassengerId = orderDto.PassengerId,
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
}