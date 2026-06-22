using System.ComponentModel.DataAnnotations;

namespace GymApp.DTOs;

public record UpdateWorkoutPlanDto(
    [Required, MinLength(3)] string Title,
    string Notes,
    List<WorkoutExerciseItemDto>? Exercises = null);
