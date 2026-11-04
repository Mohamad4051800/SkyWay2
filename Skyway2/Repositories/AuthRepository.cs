using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Skyway2.Common;
using Skyway2.Dtos;
using Skyway2.Entities;
using Skyway2.InterFaces;

namespace Skyway2.Repositories;

public class AuthRepository(
    AppDbContext _context,
    UserManager<ApplicationUser> _userManager,
    RoleManager<IdentityRole> _roleManager,
    IConfiguration _configuration) : IAuthRepository
{
    public async Task<Response<string>> RegisterPassenger(RegisterPassengerDto dto)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            var phone = dto.PhoneNumber.Trim();
            var email = dto.Email.Trim().ToLower();

            if (!phone.StartsWith("09") || phone.Length != 11 ||
                !phone.All(char.IsDigit))
            {
                return new Response<string>
                {
                    payload = null,
                    statusCode = 400,
                    message = "شماره موبایل معتبر نیست",
                    devMessage = "Invalid phone number",
                    messageLevel = MessageLevel.Admin
                };
            }

            if (!email.EndsWith("@gmail.com"))
            {
                return new Response<string>
                {
                    payload = null,
                    statusCode = 400,
                    message = "ایمیل باید متعلق به Gmail باشد",
                    devMessage = "Invalid email domain",
                    messageLevel = MessageLevel.Admin
                };
            }

            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new Response<string>
                {
                    payload = null,
                    statusCode = 409,
                    message = "این ایمیل قبلاً ثبت شده است",
                    devMessage = "Email already exists",
                    messageLevel = MessageLevel.Admin
                };
            }

            var existingPhone = await _userManager.Users
                .AnyAsync(u => u.PhoneNumber == phone);

            if (existingPhone)
            {
                return new Response<string>
                {
                    payload = null,
                    statusCode = 409,
                    message = "این شماره موبایل قبلاً ثبت شده است",
                    devMessage = "Phone number already exists",
                    messageLevel = MessageLevel.Admin
                };
            }

            var user = new ApplicationUser
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = email,
                UserName = email,
                PhoneNumber = phone
            };

            var createResult = await _userManager.CreateAsync(
                user, dto.Password);

            if (!createResult.Succeeded)
            {
                return new Response<string>
                {
                    payload = null,
                    statusCode = 400,
                    message = "ثبت‌نام ناموفق بود",
                    devMessage = string.Join(", ",
                        createResult.Errors.Select(e => e.Description)),
                    messageLevel = MessageLevel.Admin
                };
            }

            const string passengerRole = "Passenger";

            if (!await _roleManager.RoleExistsAsync(passengerRole))
            {
                var roleResult = await _roleManager.CreateAsync(
                    new IdentityRole(passengerRole));

                if (!roleResult.Succeeded)
                {
                    return new Response<string>
                    {
                        payload = null,
                        statusCode = 500,
                        message = "ایجاد نقش مسافر ناموفق بود",
                        devMessage = string.Join(", ",
                            roleResult.Errors.Select(e => e.Description)),
                        messageLevel = MessageLevel.Admin
                    };
                }
            }

            var addRoleResult = await _userManager.AddToRoleAsync(
                user, passengerRole);

            if (!addRoleResult.Succeeded)
            {
                return new Response<string>
                {
                    payload = null,
                    statusCode = 500,
                    message = "اختصاص نقش مسافر ناموفق بود",
                    devMessage = string.Join(", ",
                        addRoleResult.Errors.Select(e => e.Description)),
                    messageLevel = MessageLevel.Admin
                };
            }

            var passenger = new Passenger
            {
                ApplicationUserId = user.Id,
                DateOfBirth = dto.DateOfBirth,
                NationalCode = dto.NationalCode
            };

            _context.Passengers.Add(passenger);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new Response<string>
            {
                payload = user.Id,
                statusCode = 201,
                message = "ثبت‌نام با موفقیت انجام شد",
                devMessage = "Passenger registered successfully",
                title = "Register Success",
                messageLevel = MessageLevel.Admin
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            return new Response<string>
            {
                payload = null,
                statusCode = 500,
                message = "خطا در ثبت‌نام",
                devMessage = ex.Message,
                messageLevel = MessageLevel.Admin
            };
        }
    }

    public async Task<Response<string>> Login(LoginDto dto)
    {
        try
        {
            var email = dto.Email.Trim().ToLower();

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null ||
                !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return new Response<string>
                {
                    payload = null,
                    statusCode = 401,
                    message = "ایمیل یا رمز عبور اشتباه است",
                    devMessage = "Invalid credentials",
                    messageLevel = MessageLevel.Admin
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return new Response<string>
            {
                payload = tokenString,
                statusCode = 200,
                message = "ورود با موفقیت انجام شد",
                devMessage = "Login successful",
                title = "Login Success",
                messageLevel = MessageLevel.Admin
            };
        }
        catch (Exception ex)
        {
            return new Response<string>
            {
                payload = null,
                statusCode = 500,
                message = "خطا در ورود",
                devMessage = ex.Message,
                messageLevel = MessageLevel.Admin
            };
        }
    }
}