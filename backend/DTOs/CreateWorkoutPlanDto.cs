using System.ComponentModel.DataAnnotations;

namespace GymApp.DTOs;

public record WorkoutExerciseItemDto(
    [Required] int ExerciseId,
    [Range(1, 20)] int Sets,
    [Range(1, 100)] int Reps,
    int OrderIndex);

public record CreateWorkoutPlanDto(
    [Required, MinLength(3)] string Title,
    string Notes,
    List<WorkoutExerciseItemDto>? Exercises = null);
