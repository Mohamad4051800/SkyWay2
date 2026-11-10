using Skyway2.Common;
using Skyway2.Dtos;

namespace Skyway2.InterFaces;

public interface IOrderRepository
{
    public Task<Response<int>> CreateOrder(string userId, CreateOrderDto orderDto);
    public Task<Response<bool>> ConfirmOrderPayment(ConfirmOrderPaymentDto dto);
    Task<Response<List<OrderDto>>> GetAllOrders();

    Task<Response<OrderDto>> GetOrderById(int orderId);

    Task<Response<List<OrderDto>>> GetMyOrders(string userId);
}
