using System.ComponentModel.DataAnnotations;

namespace GymApp.DTOs;

public record CreateEnrollmentDto(
    [Required] int ClassSessionId);
