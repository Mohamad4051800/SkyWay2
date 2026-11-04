using Skyway2.Common;
using Skyway2.Dtos;
using Skyway2.Entities;

namespace Skyway2.InterFaces;

public interface IPassengerRepository
{
    public Task<Response<List<PassengerDto>>>GetPassengersAsync();
    public Task<Response<List<PassengerDto>>>GetPassengersById(int PassengerId);
   // public Task<Response<int>>CreatePassenger(CreatePassengerDto passengerDto);
    public Task<Response<bool>> UpdatePassenger(int PassengerId, UpdatePassengerDto  passengerDto);
    public Task<Response<bool>> DeletePassenger(int PassengerId);

}