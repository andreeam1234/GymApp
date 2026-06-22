using System.ComponentModel.DataAnnotations;

namespace GymApp.DTOs;

public record UpdateClassSessionDto(
    [Required] DateTime StartTime,
    [Required] DateTime EndTime,
    [Required, Range(1, 200)] int Capacity,
    string Room = "");
