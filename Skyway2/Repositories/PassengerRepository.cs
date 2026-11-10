using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Skyway2.Common;
using Skyway2.Dtos;
using Skyway2.Entities;
using Skyway2.InterFaces;

namespace Skyway2.Repositories;

public class PassengerRepository(
    AppDbContext _passenger,
    UserManager<ApplicationUser> _userManager) : IPassengerRepository{
    public async Task<Response<List<PassengerDto>>> GetPassengersAsync()
    {
        var response = new Response<List<PassengerDto>>();
        response.payload = new List<PassengerDto>();
        try
        {
            var passengers = await _passenger.Passengers
                .Include(p => p.ApplicationUser)
                .ToListAsync();
                if (!passengers.Any())
            {
                response.statusCode = -1;
                response.message = "موردی یافت نشد";
                response.devMessage = "No Data Found";
                response.title = "Get All Products";
                response.messageLevel = MessageLevel.Admin;
                return response;
            }

            foreach (var p in passengers)
            {
                response.payload.Add(new PassengerDto
                {
                    PassengerId = p.PassengerId,
                    FirstName = p.ApplicationUser.FirstName,
                    LastName = p.ApplicationUser.LastName,
                    Email = p.ApplicationUser.Email!,
                    PhoneNumber = p.ApplicationUser.PhoneNumber!,
                    DateOfBirth = p.DateOfBirth,
                    NationalCode = p.NationalCode
                });
            }

            response.statusCode = 200;
            response.message = "عملیات با موفقیت انجام شد";
            response.title = "Get All Customers";
            response.messageLevel = MessageLevel.Admin;
        }
        catch (Exception ex)
        {
            response.payload = null;
            response.statusCode = 500;
            response.message = "خطا در دریافت لیست مشتریان";
            response.devMessage = ex.Message;
            response.messageLevel = MessageLevel.Admin;
        }

        return response;
    }

    public async Task<Response<List<PassengerDto>>> GetPassengersById(int PassengerId)
    {
        var response = new Response<List<PassengerDto>>();
        response.payload = new List<PassengerDto>();

        try
        {
            var passenger = await _passenger.Passengers
                .Include(p => p.ApplicationUser)
                .FirstOrDefaultAsync(p => p.PassengerId == PassengerId);

            if (passenger == null)
            {
                response.payload = null;
                response.statusCode = 404;
                response.message = "مسافر یافت نشد";
                response.devMessage = $"Passenger with id {PassengerId} not found.";
                response.messageLevel = MessageLevel.Admin;
                return response;
            }

            response.payload.Add(new PassengerDto
            {
                PassengerId = passenger.PassengerId,
                FirstName = passenger.ApplicationUser.FirstName,
                LastName = passenger.ApplicationUser.LastName,
                Email = passenger.ApplicationUser.Email!,
                PhoneNumber = passenger.ApplicationUser.PhoneNumber!,
                DateOfBirth = passenger.DateOfBirth,
                NationalCode = passenger.NationalCode
            });

            response.statusCode = 200;
            response.message = "عملیات با موفقیت انجام شد";
            response.messageLevel = MessageLevel.Admin;
            response.title = "Get passenger By Id";
        }
        catch (Exception ex)
        {
            response.statusCode = 500;
            response.message = "خطا در دریافت اطلاعات مسافر";
            response.devMessage = ex.Message;
            response.messageLevel = MessageLevel.Admin;
            response.payload = null;
        }

        return response;
    }

   // public async Task<Response<int>> CreatePassenger(CreatePassengerDto passengerDto)
    // {
    //     var phone = passengerDto.PhoneNumber.Trim();
    //     if (string.IsNullOrEmpty(phone) || !phone.StartsWith("09") || phone.Length != 11)
    //     {
    //         return new Response<int>
    //         {
    //             payload = 0,
    //             statusCode = 400,
    //             message = "شماره موبایل معتبر نمیباشد!",
    //             devMessage = "Invalid phone format",
    //             title = " Phone Number Validation Error",
    //             messageLevel = MessageLevel.Admin
    //         };
    //     }
    //
    //     var email = passengerDto.Email.ToLower().Trim();
    //     if (string.IsNullOrEmpty(email) || !email.EndsWith("@gmail.com"))
    //     {
    //         return new Response<int>
    //         {
    //             payload = 0,
    //             statusCode = 400,
    //             message = "ایمیل وارد شده معتبر نمیباشد",
    //             devMessage = "Invalid email format - must end with @gmail.com",
    //             title = "ٍEmail Validation Error",
    //             messageLevel = MessageLevel.Admin
    //         };
    //     }
    //
    //     var passenger = new Passenger
    //     {
    //         FirstName = passengerDto.FirstName,
    //         LastName = passengerDto.LastName,
    //         Email = email,
    //         PhoneNumber = passengerDto.PhoneNumber,
    //         DateOfBirth = passengerDto.DateOfBirth,
    //         NationalCode = passengerDto.NationalCode,
    //     };
    //     _passenger.Add(passenger);
    //     await _passenger.SaveChangesAsync();
    //     var response = new Response<int>
    //     {
    //         payload = passenger.PassengerId,
    //         statusCode = 201,
    //         message = "مسافر باموفقیت ایجاد شد",
    //         devMessage = $"Passenger with id {passenger.PassengerId} was created.",
    //         title = "Create Passenger Success",
    //         messageLevel = MessageLevel.Admin
    //     };
    //     return response;
    // }


    public async Task<Response<bool>> UpdatePassenger(int PassengerId, UpdatePassengerDto  passengerDto)
    {
        var response = new Response<bool>();
        response.payload = false;

        try
        {
            var passenger = await _passenger.Passengers
                .FirstOrDefaultAsync(p => p.PassengerId == PassengerId);

            if (passenger == null)
            {
                response.statusCode = 404;
                response.message = "مسافر یافت نشد";
                response.devMessage = $"Passenger not found.";
                response.messageLevel = MessageLevel.Admin;
                return response;
            }

            passenger.DateOfBirth = passengerDto.DateOfBirth;
            passenger.NationalCode = passengerDto.NationalCode;

            await _passenger.SaveChangesAsync();

            response.payload = true;
            response.statusCode = 200;
            response.message = "اطلاعات مسافر با موفقیت بروزرسانی شد";
            response.devMessage = "Passenger updated successfully";
            response.title = "Update Passenger Success";
            response.messageLevel = MessageLevel.Admin;
        }
        catch (Exception e)
        {
            response.statusCode = 500;
            response.message = "خطا در بروزرسانی اطلاعات مسافر";
            response.devMessage = e.Message;
            response.messageLevel = MessageLevel.Admin;
        }

        return response;
    }

  public async Task<Response<bool>> DeletePassenger(int PassengerId)
{
    var response = new Response<bool>();
    response.payload = false;

    try
    {
        await using var transaction =
            await _passenger.Database.BeginTransactionAsync();

        var passenger = await _passenger.Passengers
            .Include(p => p.ApplicationUser)
            .FirstOrDefaultAsync(p => p.PassengerId == PassengerId);

        if (passenger == null)
        {
            response.statusCode = 404;
            response.message = "مسافر یافت نشد";
            response.messageLevel = MessageLevel.Admin;
            return response;
        }

        var orders = await _passenger.Orders
            .Where(o => o.PassengerId == PassengerId)
            .Include(o => o.Ticket)
            .ToListAsync();

        var tickets = orders
            .Where(o => o.Ticket != null)
            .Select(o => o.Ticket!)
            .ToList();

        // Delete Tickets
        _passenger.Tickets.RemoveRange(tickets);

        // Delete Orders
        _passenger.Orders.RemoveRange(orders);

        // Delete Passenger
        _passenger.Passengers.Remove(passenger);

        await _passenger.SaveChangesAsync();

        // Delete Identity User
        var userResult = await _userManager.DeleteAsync(
            passenger.ApplicationUser);

        if (!userResult.Succeeded)
        {
            response.statusCode = 400;
            response.message = "حذف حساب کاربری ناموفق بود";
            response.devMessage = string.Join(", ",
                userResult.Errors.Select(e => e.Description));
            response.messageLevel = MessageLevel.Admin;
            return response;
        }

        await transaction.CommitAsync();

        response.payload = true;
        response.statusCode = 200;
        response.message = "مسافر و تمام اطلاعات وابسته با موفقیت حذف شدند";
        response.devMessage = "Passenger, Orders, Tickets and Identity User deleted successfully";
        response.messageLevel = MessageLevel.Admin;
    }
    catch (Exception ex)
    {
        response.statusCode = 500;
        response.message = "خطا در حذف مسافر";
        response.devMessage = ex.Message;
        response.messageLevel = MessageLevel.Admin;
    }

    return response;
}
public async Task<Response<PassengerDto>> GetMyProfile(string userId)
{
    var response = new Response<PassengerDto>();

    try
    {
        var passenger = await _passenger.Passengers
            .Include(p => p.ApplicationUser)
            .FirstOrDefaultAsync(p => p.ApplicationUserId == userId);

        if (passenger == null)
        {
            response.statusCode = 404;
            response.message = "پروفایل مسافر یافت نشد";
            response.messageLevel = MessageLevel.Admin;
            return response;
        }

        response.payload = new PassengerDto
        {
            PassengerId = passenger.PassengerId,
            FirstName = passenger.ApplicationUser.FirstName,
            LastName = passenger.ApplicationUser.LastName,
            Email = passenger.ApplicationUser.Email!,
            PhoneNumber = passenger.ApplicationUser.PhoneNumber!,
            DateOfBirth = passenger.DateOfBirth,
            NationalCode = passenger.NationalCode
        };

        response.statusCode = 200;
        response.message = "اطلاعات پروفایل با موفقیت دریافت شد";
        response.messageLevel = MessageLevel.Admin;
    }
    catch (Exception ex)
    {
        response.statusCode = 500;
        response.message = "خطا در دریافت پروفایل";
        response.devMessage = ex.Message;
        response.messageLevel = MessageLevel.Admin;
    }

    return response;
}
public async Task<Response<bool>> UpdateMyProfile(
    string userId,
    UpdatePassengerDto passengerDto)
{
    var response = new Response<bool>
    {
        payload = false
    };

    try
    {
        var passenger = await _passenger.Passengers
            .FirstOrDefaultAsync(p => p.ApplicationUserId == userId);

        if (passenger == null)
        {
            response.statusCode = 404;
            response.message = "پروفایل مسافر یافت نشد";
            response.messageLevel = MessageLevel.Admin;
            return response;
        }

        passenger.DateOfBirth = passengerDto.DateOfBirth;
        passenger.NationalCode = passengerDto.NationalCode;

        await _passenger.SaveChangesAsync();

        response.payload = true;
        response.statusCode = 200;
        response.message = "پروفایل با موفقیت بروزرسانی شد";
        response.messageLevel = MessageLevel.Admin;
    }
    catch (Exception ex)
    {
        response.statusCode = 500;
        response.message = "خطا در بروزرسانی پروفایل";
        response.devMessage = ex.Message;
        response.messageLevel = MessageLevel.Admin;
    }

    return response;
}
}