using GymApp.Models;

namespace GymApp.DTOs;

public record MembershipDto(
    int Id,
    MembershipType Type,
    decimal Price,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive,
    string UserId,
    string UserName);
