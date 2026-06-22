using GymApp.Models;

namespace GymApp.Services;

public interface IJwtService
{
    int ExpiresInSeconds { get; }
    Task<string> GenerateTokenAsync(ApplicationUser user);
}
