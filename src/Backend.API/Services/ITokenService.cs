using Backend.API.Models;

namespace Backend.API.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}
