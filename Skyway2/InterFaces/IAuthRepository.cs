using Skyway2.Common;
using Skyway2.Dtos;

namespace Skyway2.InterFaces;

public interface IAuthRepository
{
    Task<Response<string>> RegisterPassenger(RegisterPassengerDto dto);

    Task<Response<string>> Login(LoginDto dto);
}