using Skyway2.Common;
using Skyway2.Dtos;

namespace Skyway2.InterFaces;

public interface IFlightRepository
{
    public Task <Response<List<FlightDto>>> GetAllFlights();
    public Task <Response<List<FlightDto>>>GetFlightByDestinationCityId(int destinationCityId);
    public Task<Response<List<FlightDto>>> GetFlightsByDateRange(DateTime startDate, DateTime endDate);
    public Task <Response<List<FlightDto>>>GetFlightByDepartureCityId(int departureCityId);
    public Task <Response<List<FlightDto>>>GetFlightByFlightId(int FlightId);
    public Task <Response<int>> CreateFlight(CreateFlightDto flight);
    public Task<Response<bool>> UpdateFlight(int flightId,UpdateflightDto flight);
    public Task<Response<List<FlightDto>>> SearchFlightsByDepartureCity(string searchText);
    public Task<Response<List<FlightDto>>> SearchFlightsByDestinationCity(string searchText);
    public Task<Response<bool>> DeleteFlight(int flightId);
    
}