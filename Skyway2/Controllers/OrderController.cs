using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skyway2.Dtos;
using Skyway2.InterFaces;
using System.Security.Claims;

namespace Skyway2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderRepository _order) : ControllerBase
{
    [Authorize(Roles = "Passenger")]
    [HttpPost("CreateOrder")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        var response = await _order.CreateOrder(userId, orderDto);
        return StatusCode(response.statusCode, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("ConfirmOrderPayment")]
    public async Task<IActionResult> ConfirmOrderPayment([FromBody] ConfirmOrderPaymentDto dto)
    {
        var response = await _order.ConfirmOrderPayment(dto);
        return StatusCode(response.statusCode, response);
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("GetAllOrders")]
    public async Task<IActionResult> GetAllOrders()
    {
        var response = await _order.GetAllOrders();
        return StatusCode(response.statusCode, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GetOrderById/{orderId}")]
    public async Task<IActionResult> GetOrderById(int orderId)
    {
        var response = await _order.GetOrderById(orderId);
        return StatusCode(response.statusCode, response);
    }

    [Authorize(Roles = "Passenger")]
    [HttpGet("GetMyOrders")]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        var response = await _order.GetMyOrders(userId);
        return StatusCode(response.statusCode, response);
    }
}