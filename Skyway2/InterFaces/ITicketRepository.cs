using Skyway2.Common;
using Skyway2.Dtos;

namespace Skyway2.InterFaces;

public interface ITicketRepository
{
    public Task <Response<List<TicketDto>>>  GetTickets();
    public Task<Response<List<TicketDto>>>  GetTicketsByDate(DateOnly date);
    public Task<Response<List<TicketDto>>> GetPassengerTickets(int passengerId);

}