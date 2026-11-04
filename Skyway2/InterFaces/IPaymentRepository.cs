using Skyway2.Common;

namespace Skyway2.InterFaces;

public interface IPaymentRepository
{
    public Task<Response<bool>> UpdatePaymentStatus(int ticketId);
}