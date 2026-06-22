using System.ComponentModel.DataAnnotations;

namespace GymApp.DTOs;

public record CreateExerciseDto(
    [Required, MinLength(2)] string Name,
    [Required] string MuscleGroup,
    string Description = "",
    string Equipment = "");
