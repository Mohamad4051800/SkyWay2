using Microsoft.AspNetCore.Mvc;
using Skyway2.Dtos;
using Skyway2.InterFaces;

namespace Skyway2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderRepository _order) : ControllerBase
{
    [HttpPost("CreateOrder")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
    {
        var response = await _order.CreateOrder(orderDto);
        return StatusCode(response.statusCode, response);
    }
    [HttpPut("ConfirmOrderPayment")]
    public async Task<IActionResult> ConfirmOrderPayment([FromBody] ConfirmOrderPaymentDto dto)
    {
        var response = await _order.ConfirmOrderPayment(dto);
        return StatusCode(response.statusCode, response);
    }
    
}