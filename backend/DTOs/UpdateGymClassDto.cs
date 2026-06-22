using System.ComponentModel.DataAnnotations;

namespace GymApp.DTOs;

public record UpdateGymClassDto(
    [Required, MinLength(3)] string Name,
    [Required, MinLength(10)] string Description,
    [Required, Range(10, 240)] int DurationMinutes,
    [Required] string Category);
