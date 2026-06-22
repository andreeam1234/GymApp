namespace GymApp.DTOs;

public record GymClassDto(
    int Id,
    string Name,
    string Description,
    int DurationMinutes,
    string Category,
    int SessionCount);
