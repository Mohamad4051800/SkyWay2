using Skyway2.Common;
using Skyway2.Dtos;

namespace Skyway2.InterFaces;

public interface IOrderRepository
{
    public Task<Response<int>> CreateOrder(CreateOrderDto orderDto);
    public Task<Response<bool>> ConfirmOrderPayment(ConfirmOrderPaymentDto dto);

}